
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Mmtp;
using Org.BouncyCastle.Asn1.Cms;

namespace MMSEdgeRouter;

class EdgeRouter
{

    readonly string HOST = "10.0.1.1";
    readonly int PORT = 65432;

    public Dictionary<string, Thread> ConnectionThreads = [];

    public readonly ConcurrentDictionary<string, List<MmtpMessage>> messageBuffer = [];

    public EdgeRouter(string host, int port)
    {
        HOST = host;
        PORT = port;
    }

    public void Run()
    {
        Console.WriteLine($"Edgerouter running at {HOST} : {PORT}");
        Thread trimThread = new Thread(TrimTimeoutMessages);
        trimThread.Start();

        while (true)
        {
            ListenForConnection();
        }
    }

    public void ListenForConnection()
    {
        try
        {
            // Initializing socket and stream
            IPAddress iPAddress = IPAddress.Parse(HOST);
            TcpListener listener = new TcpListener(iPAddress, PORT);
            listener.Start();
            Socket s = listener.AcceptSocket();
            Stream stm = new NetworkStream(s, ownsSocket: true);
            // Getting connection message

            MmtpMessage connectMessage = ReadMmtpFromStream(stm);

            string responseToUuid = connectMessage.Uuid;
            string mrn = connectMessage.ProtocolMessage.ConnectMessage.OwnMrn;

            Console.WriteLine($"connecting to: {mrn}");
            Console.WriteLine($"At: {s.RemoteEndPoint}");

            MmtpMessage response = new MmtpMessage
            {
                MsgType = MsgType.RedponseMessage,
                Uuid = Guid.NewGuid().ToString(),
                ResponseMessage = new ResponseMessage
                {
                    ResponseToUuid = responseToUuid,
                    Response = ResponseEnum.Good
                }
            };

            response.WriteTo(stm);

            Thread connection = new Thread(() => ConnectionThread(s, mrn));
            connection.Start();
            ConnectionThreads.Add(mrn, connection);
            listener.Stop();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static MmtpMessage ReadMmtpFromStream(Stream stm)
    {
        byte[] buffer = new byte[1024];
        int k = stm.Read(buffer, 0, 1024);
        MmtpMessage connectMessage = MmtpMessage.Parser.ParseFrom(buffer, 0, k);
        return connectMessage;
    }

    private void ConnectionThread(Socket socket, string mrn)
    {
        AgentConnection connection = new AgentConnection(mrn, socket, messageBuffer);
        connection.Run();
        ConnectionThreads.Remove(mrn);
    }

    private void TrimTimeoutMessages()
    {
        while (true)
        {
            Thread.Sleep(1000);
            foreach (List<MmtpMessage> messageList in messageBuffer.Values)
            {
                for (int i = messageList.Count - 1; i >= 0; i--)
                {
                    long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    long expiers = messageList[i].
                        ProtocolMessage.
                        SendMessage.
                        ApplicationMessage.
                        Header.
                        Expires;
                    if (currentTime > expiers)
                    {
                        Console.WriteLine($"deleted {messageList[i].Uuid}");
                        Console.WriteLine($"    {currentTime} > {expiers}");

                        messageList.RemoveAt(i);
                    }
                }
            }
        }
    }
}

