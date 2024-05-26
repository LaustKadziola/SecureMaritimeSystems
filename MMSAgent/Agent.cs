using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using Mmtp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Digests;
using System.Formats.Asn1;

namespace MMSAgent;

public class Agent
{
    public bool Verbose { get; set; } = false;

    const string HOST = "10.0.1.1";
    const int PORT = 65432;

    static string pathPrivate => Path.Combine(
        Directory.GetCurrentDirectory(),
        "Certificates", $"private-{ownMrn}.prm");
    static string pathPublic => Path.Combine(
        Directory.GetCurrentDirectory(),
        "Certificates", $"Public-{ownMrn}.prm");

    static string ownMrn = "someMrn";

    TcpClient tcpClient = new TcpClient();

    public Agent(string mrn)
    {
        ownMrn = mrn;
        //MakeCert();
        Log("Starting agent");
    }

    public string ConnectAuthenticated(string mrnEdgeRouter, string certificate)
    {
        MmtpMessage response;
        MmtpMessage message = new MmtpMessage
        {
            MsgType = MsgType.ProtocolMessage,
            Uuid = Guid.NewGuid().ToString(),
            ProtocolMessage = new ProtocolMessage
            {
                ProtocolMsgType = ProtocolMessageType.ConnectMessage,
                ConnectMessage = new Connect
                {
                    OwnMrn = ownMrn,
                }
            }
        };


        try
        {
            tcpClient = tcpClient.Connected ? tcpClient : new TcpClient();
            Log($"Connecting to {HOST} {PORT}");
            tcpClient.Connect(HOST, PORT);

            Stream stm = tcpClient.GetStream();
            message.WriteTo(stm);

            response = ReadMmtpFromStream(stm);
            return response.ResponseMessage.Response.ToString();
        }
        catch (Exception e)
        {
            Log(e);
            return "Error";
        }
    }

    public string Send(int TTL, List<string> MRN, string message)
    {
        MmtpMessage response;
        Recipients recipients = new Recipients();
        recipients.Recipients_.Add(MRN);

        string mrnStr = "To";
        foreach (string mrn in MRN) { mrnStr += $": {mrn}"; }
        Log(mrnStr);

        ByteString body = ByteString.CopyFrom(message, Encoding.UTF8);
        uint length = (uint)body.Length;

        Log("own " + ownMrn);

        MmtpMessage SendMessage = new MmtpMessage
        {
            MsgType = MsgType.ProtocolMessage,
            Uuid = Guid.NewGuid().ToString(),
            ProtocolMessage = new ProtocolMessage
            {
                ProtocolMsgType = ProtocolMessageType.SendMessage,
                SendMessage = new Send
                {
                    ApplicationMessage = new ApplicationMessage
                    {
                        Body = body,
                        Signature = "someSig",
                        Header = new ApplicationMessageHeader
                        {
                            Recipients = recipients,
                            Expires = TTL,
                            Sender = ownMrn,
                            BodySizeNumBytes = length
                        }
                    }
                }
            }
        };

        string signature = GenerateSignature(SendMessage.ProtocolMessage.SendMessage.ApplicationMessage);
        Log(signature);
        SendMessage.ProtocolMessage.SendMessage.ApplicationMessage.Signature = signature;

        //SendMessage.ProtocolMessage.SendMessage.ApplicationMessage.Header.Sender = "sdfsdf";


        try
        {
            Stream stm = tcpClient.GetStream();
            SendMessage.WriteTo(stm);
            response = ReadMmtpFromStream(stm);

            return response.ResponseMessage.Response.ToString();
        }
        catch (Exception e)
        {
            Log(e);
            return ResponseEnum.Error.ToString();
        }

    }

    public string Receive(string filter = "")
    {
        MmtpMessage response;
        MmtpMessage recieveMessage = new MmtpMessage
        {
            MsgType = MsgType.ProtocolMessage,
            Uuid = Guid.NewGuid().ToString(),
            ProtocolMessage = new ProtocolMessage
            {
                ProtocolMsgType = ProtocolMessageType.RecieveMessage
            }
        };

        try
        {
            Stream stm = tcpClient.GetStream();
            recieveMessage.WriteTo(stm);
            response = ReadMmtpFromStream(stm);

            Log(response.ToString());

            if (!VerifyeSignature(response.ResponseMessage.ApplicationMessage[0]))
            {
                Log("Invalid signature");
            }

            return response.ResponseMessage.ApplicationMessage[0].Body.ToStringUtf8();
        }
        catch (Exception e)
        {
            Log(e);
            return ResponseEnum.Error.ToString();
        }
    }

    public string Disconnect()
    {
        MmtpMessage response;
        MmtpMessage DisconnectMessage = new MmtpMessage
        {
            MsgType = MsgType.ProtocolMessage,
            Uuid = Guid.NewGuid().ToString(),
            ProtocolMessage = new ProtocolMessage
            {
                ProtocolMsgType = ProtocolMessageType.DisconnectMessage
            }
        };

        try
        {
            Stream stm = tcpClient.GetStream();
            DisconnectMessage.WriteTo(stm);
            response = ReadMmtpFromStream(stm);

            tcpClient.Close();

            return response.ResponseMessage.Response.ToString();
        }
        catch (Exception e)
        {
            Log(e);
            return ResponseEnum.Error.ToString();
        }
    }

    private static MmtpMessage ReadMmtpFromStream(Stream stm)
    {
        byte[] buffer = new byte[1024];
        int k = stm.Read(buffer, 0, 1024);
        MmtpMessage connectMessage = MmtpMessage.Parser.ParseFrom(buffer, 0, k);
        return connectMessage;
    }

    private void MakeCert()
    {
        var curvename = "secp256k1";

        X9ECParameters par = ECNamedCurveTable.GetByName(curvename);

        var curParam = new ECDomainParameters(par.Curve, par.G, par.N, par.H, par.GetSeed());

        ECKeyGenerationParameters keyGenParam = new ECKeyGenerationParameters(curParam, new SecureRandom());

        ECKeyPairGenerator generator = new ECKeyPairGenerator();

        generator.Init(keyGenParam);

        AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var certGenerator = new X509V3CertificateGenerator();

        certGenerator.SetSubjectDN(new X509Name($"CN={ownMrn}"));
        certGenerator.SetNotAfter(DateTime.UtcNow.AddDays(30));
        certGenerator.SetNotBefore(DateTime.UtcNow);
        certGenerator.SetPublicKey(publicKey);
        certGenerator.SetIssuerDN(new X509Name($"CN={ownMrn}"));
        certGenerator.SetSerialNumber(BigInteger.ValueOf(1));

        var algorithm = X9ObjectIdentifiers.ECDsaWithSha256.ToString();
        var cert = certGenerator.Generate(new Asn1SignatureFactory(algorithm, privateKey));


        Log($"Valid: {ValidateSignature(cert, publicKey)}");

        PemWriter pemWriterPrivate = new PemWriter(new StreamWriter(pathPrivate));
        pemWriterPrivate.WriteObject(privateKey);
        pemWriterPrivate.Writer.Flush();
        pemWriterPrivate.Writer.Close();

        PemWriter pemWriterPublic = new PemWriter(new StreamWriter(pathPublic));
        pemWriterPublic.WriteObject(publicKey);
        pemWriterPublic.Writer.Flush();
        pemWriterPublic.Writer.Close();
    }


    private bool ValidateSignature(X509Certificate cert, ICipherParameters publicKey)
    {
        cert.CheckValidity(DateTime.UtcNow);
        var tbsCert = cert.GetTbsCertificate();
        var signature = cert.GetSignature();
        var signer = SignerUtilities.GetSigner(cert.SigAlgName);
        signer.Init(false, publicKey);
        signer.BlockUpdate(tbsCert, 0, tbsCert.Length);
        return signer.VerifySignature(signature);
    }

    private void Log(object x)
    {
        if (Verbose) { Console.WriteLine(x); }
    }

    private static string GenerateSignature(ApplicationMessage message)
    {
        byte[] messageHash = GenerateHash(message);

        // var curvename = "secp256k1";

        // X9ECParameters ecParams = ECNamedCurveTable.GetByName(curvename);
        // var curveparam = new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());

        // ECKeyGenerationParameters keygenParams = new ECKeyGenerationParameters(curveparam, new SecureRandom());

        // ECKeyPairGenerator generator = new ECKeyPairGenerator("ECDSA");
        // generator.Init(keygenParams);
        // var keyPair = generator.GenerateKeyPair();

        ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));

        PemReader pemReader = new PemReader(new StreamReader(pathPrivate));
        AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();

        signer.Init(true, keyPair.Private);

        var signature = signer.GenerateSignature(messageHash);

        // Converting to Asn1
        AsnWriter ansWriter = new(AsnEncodingRules.DER);
        ansWriter.WriteInteger(signature[0].ToByteArray(), Asn1Tag.Integer);
        ansWriter.WriteInteger(signature[1].ToByteArray(), Asn1Tag.Integer);
        byte[] asn1Bytes = ansWriter.Encode();

        return Convert.ToBase64String(asn1Bytes);
    }

    private static byte[] GenerateHash(ApplicationMessage message)
    {
        List<byte> bytes = [];
        foreach (string recipient in message.Header.Recipients.Recipients_)
        {
            bytes.AddRange(Encoding.UTF8.GetBytes(recipient));
        }

        bytes.AddRange(Encoding.UTF8.GetBytes(message.Header.Expires.ToString()));
        bytes.AddRange(Encoding.UTF8.GetBytes(message.Header.Sender));

        bytes.AddRange(Encoding.UTF8.GetBytes(message.Header.QosProfile));
        bytes.AddRange(Encoding.UTF8.GetBytes(message.Header.BodySizeNumBytes.ToString()));
        bytes.AddRange(message.Body);

        var h13 = new Sha256Digest();
        h13.BlockUpdate(bytes.ToArray(), 0, bytes.Count);
        var messageHash = new byte[h13.GetDigestSize()];
        h13.DoFinal(messageHash, 0);
        return messageHash;
    }

    private static bool VerifyeSignature(ApplicationMessage message)
    {
        byte[] messageHash = GenerateHash(message);
        ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));

        PemReader pemReader = new PemReader(new StreamReader(pathPrivate));
        AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();

        AsnReader asnReader = new(Convert.FromBase64String(message.Signature), AsnEncodingRules.DER);

        signer.Init(false, keyPair.Public);

        BigInteger r = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        BigInteger s = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        bool res = signer.VerifySignature(messageHash, r, s);

        return res;
    }

}