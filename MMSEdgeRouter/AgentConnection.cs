using System.Net.Sockets;
using Mmtp;
using Google.Protobuf;
using System.Collections.Concurrent;

namespace MMSEdgeRouter;

class AgentConnection
{
    Socket socket;
    private bool isRunning = true;

    readonly string agentMrn;

    public readonly ConcurrentDictionary<string, List<MmtpMessage>> messageBuffer;

    public AgentConnection(string mrn, Socket s, ConcurrentDictionary<string, List<MmtpMessage>> messageBuffer)
    {
        this.agentMrn = mrn;
        this.messageBuffer = messageBuffer;
        socket = s;
    }

    public void Run()
    {
        while (isRunning)
        {
            try
            {
                byte[] buf = new byte[1024];
                Stream stm = new NetworkStream(socket, ownsSocket: true);
                MmtpMessage messageRecv = ReadMmtpFromStream(stm);

                MmtpMessage response = messageRecv.ProtocolMessage.ProtocolMsgType switch
                {
                    ProtocolMessageType.Unspecified => throw new NotImplementedException(),
                    ProtocolMessageType.SubscribeMessage => throw new NotImplementedException(),
                    ProtocolMessageType.UnsubcribeMessage => throw new NotImplementedException(),
                    ProtocolMessageType.SendMessage => HandleSendMessage(messageRecv),
                    ProtocolMessageType.RecieveMessage => HandleReceiveMessage(messageRecv),
                    ProtocolMessageType.FetchMessage => throw new NotImplementedException(),
                    ProtocolMessageType.DisconnectMessage => HandleDisconnectMessage(messageRecv),
                    ProtocolMessageType.ConnectMessage => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };

                response.WriteTo(stm);
            }
            catch (IOException)
            {
                Console.WriteLine($"Connection reset by: {agentMrn} at:{socket.RemoteEndPoint}");
                isRunning = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isRunning = false;
            }
        }

        socket.Close();
    }

    private MmtpMessage HandleReceiveMessage(MmtpMessage message)
    {
        Console.WriteLine("Recived Recieve message");

        MmtpMessage response = new MmtpMessage
        {
            Uuid = Guid.NewGuid().ToString(),
            MsgType = MsgType.RedponseMessage,
            ResponseMessage = new ResponseMessage
            {
                Response = ResponseEnum.Good,
                ResponseToUuid = message.Uuid,
            }

        };

        if (!messageBuffer.ContainsKey(agentMrn))
        {
            Console.WriteLine($" - Buffer diden't have key {agentMrn}");
            return response;
        };

        foreach (MmtpMessage m in messageBuffer[agentMrn])
        {
            ApplicationMessage applicationMessage = m.ProtocolMessage.SendMessage.ApplicationMessage;
            response.ResponseMessage.ApplicationMessage.Add(applicationMessage);
        }

        return response;
    }

    private MmtpMessage HandleSendMessage(MmtpMessage message)
    {
        Console.WriteLine("Recived Send message");

        var recipients = message
            .ProtocolMessage
            .SendMessage
            .ApplicationMessage
            .Header.Recipients.Recipients_;

        foreach (string recipient in recipients)
        {
            if (!messageBuffer.ContainsKey(recipient))
            {
                messageBuffer[recipient] = [];
            }
            Console.WriteLine($" - To mrn: {recipient}");
            messageBuffer[recipient].Add(message);
        }

        MmtpMessage response = new MmtpMessage
        {
            Uuid = Guid.NewGuid().ToString(),
            MsgType = MsgType.RedponseMessage,
            ResponseMessage = new ResponseMessage
            {
                Response = ResponseEnum.Good,
                ResponseToUuid = message.Uuid
            }

        };

        return response;
    }

    private MmtpMessage HandleDisconnectMessage(MmtpMessage message)
    {
        Console.WriteLine("Recived Disconnect message");

        MmtpMessage response = new MmtpMessage
        {
            Uuid = Guid.NewGuid().ToString(),
            MsgType = MsgType.RedponseMessage,
            ResponseMessage = new ResponseMessage
            {
                Response = ResponseEnum.Good,
                ResponseToUuid = message.Uuid
            }
        };

        isRunning = false;
        return response;
    }

    private static MmtpMessage ReadMmtpFromStream(Stream stm)
    {
        MmtpMessage connectMessage = new MmtpMessage();
        int k = 0;
        byte[] buffer = new byte[1024];
        k = stm.Read(buffer, 0, 1024);
        connectMessage = MmtpMessage.Parser.ParseFrom(buffer, 0, k);
        return connectMessage;
    }
}