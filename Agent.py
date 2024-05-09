#Agent

import socket
import json
import messages_pb2
import uuid

HOST = "10.0.1.1"
PORT = 65432


def DiscoverEdgeRouters():
    # Shall return list of Mrn of all mms edge routers
    pass

def ConnectAnonymously(MRN_edge_router):
    # Connects to a edgerouter vis MRN
    pass

def ReconnectAnonymously(MRN_edge_router, reconnection_token):
    # arguments ?
    pass

def ConnectAuthenticated(MRN_edge_router, certificate):
    message = messages_pb2.MmtpMessage()
    message.msgType = messages_pb2.PROTOCOL_MESSAGE
    message.uuid = str(uuid.uuid4())

    message.protocolMessage.protocolMsgType = messages_pb2.ProtocolMessageType.CONNECT_MESSAGE

    #protocolMessage.connectMessage = messages_pb2.Connect()
    message.protocolMessage.connectMessage.ownMrn = MRN_edge_router

    message_str = message.SerializeToString()

    print(message_str)
    print(b"Hello, world")

    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((HOST, PORT))
        s.sendall(message_str)
        data = s.recv(1024)

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

def Send(TTL, MRN, message):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((HOST, PORT))
        s.sendall(b"Hello, world")
        data = s.recv(1024)
    pass

def Notify():
    pass

def Receive(filter):
    pass

def Disconnect():
    pass

ConnectAuthenticated("someMrn", "someCertificate")

#with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
#    s.connect((HOST, PORT))
#    s.sendall(b"Hello, world")
#    data = s.recv(1024)

#print("Recived " + str(data))
