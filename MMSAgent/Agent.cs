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

namespace MMSAgent;

public class Agent
{

    const string HOST = "10.0.1.1";
    const int PORT = 65432;

    const string MDNS_ADDRESS = "224.0.0.251";
    const int MDNS_PORT = 5353;


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
        Console.WriteLine("Starting agent");
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
                    OwnMrn = mrnEdgeRouter,
                }
            }
        };


        try
        {
            if (!tcpClient.Connected)
            {
                tcpClient = new TcpClient();
            }

            Console.WriteLine($"Connecting to {HOST} {PORT}");
            tcpClient.Connect(HOST, PORT);

            Stream stm = tcpClient.GetStream();
            message.WriteTo(stm);

            response = ReadMmtpFromStream(stm);
            return response.ResponseMessage.Response.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Error";
        }
    }

    public string Send(int TTL, List<string> MRN, string message)
    {
        MmtpMessage response;
        Recipients recipients = new Recipients();
        recipients.Recipients_.Add(MRN);

        ByteString body = ByteString.CopyFrom(message, Encoding.UTF8);
        uint length = (uint)body.Length;

        Console.WriteLine("own " + ownMrn);

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

        try
        {
            Stream stm = tcpClient.GetStream();
            SendMessage.WriteTo(stm);
            response = ReadMmtpFromStream(stm);

            return response.ResponseMessage.Response.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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

            return response.ResponseMessage.ApplicationMessage[0].Body.ToStringUtf8();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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
            Console.WriteLine(e);
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


        Console.WriteLine($"Valid: {ValidateSignature(cert, publicKey)}");

        PemWriter pemWriterPrivate = new PemWriter(new StreamWriter(pathPrivate));
        pemWriterPrivate.WriteObject(privateKey);
        pemWriterPrivate.Writer.Flush();
        pemWriterPrivate.Writer.Close();

        PemWriter pemWriterPublic = new PemWriter(new StreamWriter(pathPublic));
        pemWriterPublic.WriteObject(privateKey);
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

}
