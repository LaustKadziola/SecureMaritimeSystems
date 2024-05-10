#Agent

import socket
import json
import messages_pb2
import uuid
import sys

HOST = "10.0.1.1"
PORT = 65432

MRN_OWN = "someMrn"

class Agent:
    def __init__(self):
        pass

    def DiscoverEdgeRouters():
        # Shall return list of Mrn of all mms edge routers
        pass

    def ConnectAnonymously(MRN_edge_router):
        # Connects to a edgerouter vis MRN
        pass

    def ReconnectAnonymously(MRN_edge_router, reconnection_token):
        # arguments ?
        pass

    def ConnectAuthenticated(self, MRN_edge_router, certificate):
        message = messages_pb2.MmtpMessage()
        message.msgType = messages_pb2.PROTOCOL_MESSAGE
        message.uuid = str(uuid.uuid4())
        message.protocolMessage.protocolMsgType = messages_pb2.ProtocolMessageType.CONNECT_MESSAGE
        message.protocolMessage.connectMessage.ownMrn = MRN_edge_router

        message_str = message.SerializeToString()

        self.EdgeRouter = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.EdgeRouter.connect((HOST, PORT))
        self.EdgeRouter.sendall(message_str)
        data = self.EdgeRouter.recv(1024)

        message = messages_pb2.MmtpMessage()
        message.ParseFromString(data)
        #with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        #    s.connect((HOST, PORT))
        #    s.sendall(message_str)
        #    data = s.recv(1024)

        print(data)
        pass

    def ReconnectAuthenticated(MRN_edge_router, reconnection_token):
        pass

    def Status():
        pass

    def Query():
        pass

    def Subscribe(subject):
        pass

    def Unsubscribe(subject):
        pass

    def Subscribe():
        pass

    def Unsubscribe():
        pass

    def Send(self, TTL, MRN, message):
        mmtpMessage = messages_pb2.MmtpMessage()

        mmtpMessage.msgType = messages_pb2.PROTOCOL_MESSAGE
        mmtpMessage.uuid = str(uuid.uuid4())
        mmtpMessage.protocolMessage.protocolMsgType = messages_pb2.ProtocolMessageType.SEND_MESSAGE

        applicationMessage = mmtpMessage.protocolMessage.sendMessage.applicationMessage
        applicationMessage.body = message.encode('utf-8')
        applicationMessage.signature = "someSig"

        applicationMessageHeader = applicationMessage.header

        applicationMessageHeader.recipients.recipients.extend([MRN])
        applicationMessageHeader.expires = TTL
        applicationMessageHeader.sender = MRN_OWN
        applicationMessageHeader.bodySizeNumBytes = len(applicationMessage.body)

        message_str = mmtpMessage.SerializeToString()

        self.EdgeRouter.sendall(message_str)
    #    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            #s.connect((HOST, PORT))
            #s.sendall(message_str)

        data = self.EdgeRouter.recv(1024)

        print(data)
        pass

    def Notify():
        pass

    def Receive(self, filter=""):
        mmtpMessage = messages_pb2.MmtpMessage()
        mmtpMessage.msgType = messages_pb2.PROTOCOL_MESSAGE
        mmtpMessage.uuid = str(uuid.uuid4())
        mmtpMessage.protocolMessage.protocolMsgType = messages_pb2.ProtocolMessageType.RECIEVE_MESSAGE
        message_str = mmtpMessage.SerializeToString()

        self.EdgeRouter.sendall(message_str)

        data = self.EdgeRouter.recv(1024)
        messageRecv = messages_pb2.MmtpMessage()
        messageRecv.ParseFromString(data)

        messages = messageRecv.prsponseMessage.applicationMessage

        for m in messages:
            print(m.body.decode('utf-8'))
        print(messageRecv.prsponseMessage.response)
        pass

    def Disconnect():
        pass

if __name__ == "__main__":
    if(len(sys.argv) > 1):
        MRN_OWN = sys.argv[1]

    agent = Agent()

    agent.ConnectAuthenticated(MRN_OWN, "someCertificate")

    isRunning = True;
    while (isRunning):
        command = input('Enter command :\n')
        if (command == "-s"):
            content = input('Enter message :\n')
            MRN = input('Enter MRN of reciver :\n')
            agent.Send(100000, MRN, content)
        elif (command == "-r"):
            print("reciving messages")
            agent.Receive()
        elif (command == "exit"):
            print("shutting down")
            isRunning = False
        else:
            print("unknown command")


#with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
#    s.connect((HOST, PORT))
#    s.sendall(b"Hello, world")
#    data = s.recv(1024)

#print("Recived " + str(data))
