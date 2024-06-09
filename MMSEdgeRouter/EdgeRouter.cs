
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

    public readonly ConcurrentDictionary<string, ConcurrentDictionary<string, MmtpMessage>> messageBuffer = [];

    public EdgeRouter(string host, int port)
    {
        Console.WriteLine(Environment.ProcessorCount);
        HOST = host;
        PORT = port;
    }

    public void Run()
    {
        Console.WriteLine($"Edgerouter running at {HOST} : {PORT}");
        Thread trimThread = new Thread(TrimTimeoutMessages);
        trimThread.Start();

        ListenForConnection();

    }

    public void ListenForConnection()
    {
        // Initializing socket and stream
        IPAddress iPAddress = IPAddress.Parse(HOST);
        TcpListener listener = new TcpListener(iPAddress, PORT);
        listener.Start();
        while (true)
        {
            try
            {
                Socket s = listener.AcceptSocket();
                Thread connection = new Thread(() => ConnectionThread(s));
                connection.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private static MmtpMessage ReadMmtpFromStream(Stream stm)
    {
        byte[] buffer = new byte[1024];
        int k = stm.Read(buffer, 0, 1024);
        MmtpMessage connectMessage = MmtpMessage.Parser.ParseFrom(buffer, 0, k);
        return connectMessage;
    }

    private void ConnectionThread(Socket socket)
    {
        try
        {
            Stream stm = new NetworkStream(socket, ownsSocket: true);
            // Getting connection message
            stm.WriteTimeout = 1000;
            MmtpMessage connectMessage = ReadMmtpFromStream(stm);

            string responseToUuid = connectMessage.Uuid;
            string mrn = connectMessage.ProtocolMessage.ConnectMessage.OwnMrn;


            Console.WriteLine($"connecting to: {mrn}");
            Console.WriteLine($"At: {socket.RemoteEndPoint}");

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
            AgentConnection connection = new AgentConnection(mrn, socket, messageBuffer);
            //ConnectionThreads.Add(mrn, connection);
            connection.Run();
            ConnectionThreads.Remove(mrn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    private void TrimTimeoutMessages()
    {
        while (true)
        {
            Thread.Sleep(1000);
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            foreach (ConcurrentDictionary<string, MmtpMessage> messageDict in messageBuffer.Values)
            {
                foreach (MmtpMessage message in messageDict.Values)
                {
                    long expiers = message.
                        ProtocolMessage.
                        SendMessage.
                        ApplicationMessage.
                        Header.
                        Expires;

                    if (currentTime > expiers)
                    {
                        Console.WriteLine($"deleted {message.Uuid}");
                        Console.WriteLine($"    {currentTime} > {expiers}");

                        messageDict.Remove(message.Uuid, out _);
                    }
                }
            }
        }
    }
}

