using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using Mmtp;
using Org.BouncyCastle.Crypto;
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

    private List<string> messagesForFilter = [];
    private Dictionary<string, long> recievedMessages = [];

    public string ownMrn { get; private set; } = "someMrn";

    TcpClient tcpClient = new TcpClient();

    public Agent(string mrn)
    {
        ownMrn = mrn;
        Log("Starting agent {mrn}");
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

    public string Status
    {
        get
        {
            if (tcpClient.Connected)
            {
                return "CONNECTED";
            }

            return "NOT CONNECTED";
        }
    }

    public string Send(long TTL, List<string> MRN, string message)
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

        //The code that tampers with the message.
        //ByteString bodyTampered = ByteString.CopyFrom(message + "wrong stuff", Encoding.UTF8);
        //SendMessage.ProtocolMessage.SendMessage.ApplicationMessage.Body = bodyTampered;

        try
        {
            string signature = GenerateSignature(SendMessage.ProtocolMessage.SendMessage.ApplicationMessage);
            Log(signature);
            SendMessage.ProtocolMessage.SendMessage.ApplicationMessage.Signature = signature;

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

    public List<string> Receive()
    {
        long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        foreach ((string id, long ttl) in recievedMessages.ToList())
        {
            if (ttl < currentTime) { recievedMessages.Remove(id); }
        }

        Filter filter = new Filter();
        filter.MessageUuids.AddRange(messagesForFilter);

        MmtpMessage response;
        MmtpMessage recieveMessage = new MmtpMessage
        {
            MsgType = MsgType.ProtocolMessage,
            Uuid = Guid.NewGuid().ToString(),
            ProtocolMessage = new ProtocolMessage
            {
                ProtocolMsgType = ProtocolMessageType.RecieveMessage,
                ReceiveMessage = new Receive
                {
                    Filter = filter
                }
            }
        };

        try
        {
            Stream stm = tcpClient.GetStream();
            recieveMessage.WriteTo(stm);
            response = ReadMmtpFromStream(stm);

            Log(response.ToString());


            List<string> result = [];

            // The edge router have responded 
            // and should therefor have deleted the messages from filter
            // and therefor should we be able to delete the messages.
            messagesForFilter.Clear();
            result.Add(response.ResponseMessage.Response.ToString());

            for (int i = 0; i < response.ResponseMessage.ApplicationMessage.Count; i++)
            {
                MessageMetadata metadata = response.ResponseMessage.MessageMetadata[i];
                ApplicationMessage m = response.ResponseMessage.ApplicationMessage[i];

                Log($"processing message with body {m.Body.ToStringUtf8()}");

                long ttl = metadata.Header.Expires;
                string uuid = metadata.Uuid;

                // The message have expired and we ignore it.
                if (ttl < currentTime)
                {
                    Log($"message have expired {uuid}");
                    continue;
                }
                // The messate have aready been recieved and we ignore it
                if (recievedMessages.ContainsKey(uuid))
                {
                    Log($"received duplicate message {uuid}");
                    continue;
                }

                messagesForFilter.Add(uuid);
                recievedMessages.Add(uuid, ttl);

                Console.WriteLine("hello");
                if (VerifyeSignature(m))
                {
                    result.Add(m.Body.ToStringUtf8());
                }
                else
                {
                    Log("Invalid signature");
                }
            }

            return result;

        }
        catch (Exception e)
        {
            Log(e);
            return [ResponseEnum.Error.ToString()];
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
        List<byte> m = [];
        int i = 0;
        while (true)
        {
            int k = stm.Read(buffer, 0, 1024);
            m.AddRange(buffer[0..k]);

            try
            {
                MmtpMessage connectMessage = MmtpMessage.Parser.ParseFrom(m.ToArray(), 0, m.Count);
                return connectMessage;
            }
            catch (Exception)
            {
                //Console.WriteLine($"Failed Passing message iteration: {i}");
                i++;
            }
        }

    }

    // private static bool ValidateCertificate(X509Certificate cert, ICipherParameters publicKey)
    // {
    //     cert.CheckValidity(DateTime.UtcNow);
    //     var tbsCert = cert.GetTbsCertificate();
    //     var signature = cert.GetSignature();
    //     var signer = SignerUtilities.GetSigner(cert.SigAlgName);
    //     signer.Init(false, publicKey);
    //     signer.BlockUpdate(tbsCert, 0, tbsCert.Length);
    //     return signer.VerifySignature(signature);
    // }

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
        string filePath = Utils.GetpathPublic(message.Header.Sender);
        Console.WriteLine($"Checking file {filePath}");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File didnt exist {filePath}");
            return false;
        }


        byte[] messageHash = GenerateHash(message);
        AsnReader asnReader = new(Convert.FromBase64String(message.Signature), AsnEncodingRules.DER);

        // Get the public key
        PemReader pemReader = new PemReader(new StreamReader(filePath));
        ECPublicKeyParameters publicKeyParameters = (ECPublicKeyParameters)pemReader.ReadObject();

        ECDsaSigner signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
        signer.Init(false, publicKeyParameters);

        BigInteger r = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        BigInteger s = new BigInteger(asnReader.ReadIntegerBytes().ToArray());
        bool res = signer.VerifySignature(messageHash, r, s);

        return res;
    }
}