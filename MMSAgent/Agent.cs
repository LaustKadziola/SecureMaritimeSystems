using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using Mmtp;

namespace MMSAgent;

public class Agent
{
    const string HOST = "10.0.1.1";
    const int PORT = 65432;

    string ownMrn = "";

    TcpClient tcpClient = new TcpClient();

    public Agent()
    {
        Console.WriteLine("Starting agent");

        bool isRunning = true;

        string quit()
        {
            isRunning = false;
            return "quiting";
        }

        while (isRunning)
        {
            Console.WriteLine(tcpClient.Connected);
            Console.Write("Input Command: ");
            string str = Console.ReadLine() ?? string.Empty;
            string response = str switch
            {
                "c" => ConnectHelper(),
                "s" => SendHelper(),
                "q" => quit(),
                "r" => Receive(),
                "d" => Disconnect(),
                _ => "Undefined Command"
            };

            Console.WriteLine(response);
        }
    }

    private string ConnectHelper()
    {
        Console.WriteLine("Set MRN: ");
        string mrn = Console.ReadLine() ?? "";
        return ConnectAuthenticated(mrn, "someSertificate");
    }

    private string SendHelper()
    {
        Console.Write("Message: ");
        string message = Console.ReadLine() ?? string.Empty;
        Console.Write("MRN: ");
        List<string> mrn = [Console.ReadLine() ?? "undefined"];
        return Send(0, mrn, message);
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

}
