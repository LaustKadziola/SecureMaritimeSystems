using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using Mmtp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Digests;
using System.Formats.Asn1;
using Org.BouncyCastle.Crypto.Parameters;

namespace MMSAgent;

public class Agent
{
    public bool Verbose { get; set; } = false;

    const string HOST = "10.0.1.1";
    const int PORT = 65432;

    string ownMrn = "someMrn";

    TcpClient tcpClient = new TcpClient();

    public Agent(string mrn)
    {
        ownMrn = mrn;
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

        //ByteString bodyTampered = ByteString.CopyFrom(message + "wrong stuff", Encoding.UTF8);
        //SendMessage.ProtocolMessage.SendMessage.ApplicationMessage.Body = bodyTampered;

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




    private static bool ValidateCertificate(X509Certificate cert, ICipherParameters publicKey)
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

        ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));

        PemReader pemReader = new PemReader(new StreamReader(Utils.GetpathPrivate(message.Header.Sender)));
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
        AsnReader asnReader = new(Convert.FromBase64String(message.Signature), AsnEncodingRules.DER);

        // Get the public key
        PemReader pemReader = new PemReader(new StreamReader(Utils.GetpathPublic(message.Header.Sender)));
        ECPublicKeyParameters publicKeyParameters = (ECPublicKeyParameters)pemReader.ReadObject();

        ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
        signer.Init(false, publicKeyParameters);

        BigInteger r = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        BigInteger s = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        bool res = signer.VerifySignature(messageHash, r, s);

        return res;
    }

}