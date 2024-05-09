#Server

import socket
import json
import messages_pb2

HOST = "10.0.1.1"
PORT = 65432

connections = dict()
subscriptions = dict()

def StartUp():
    pass

def Send(MRN, subject):
    pass

def Notify_from(From_Agent):
    pass

def Notify_to(To_Agent):
    pass


def Listen():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        conn, addr = s.accept()
        with conn:
            (ip_addr, port) = addr
            print("Connected by " + str(ip_addr) + ", " + str(port))
            while True:
                data = conn.recv(1024)
                message = messages_pb2.MmtpMessage()
                message.ParseFromString(data)

                if(message.msgType == messages_pb2.MsgType.PROTOCOL_MESSAGE):
                    print("ProtocolMessage")
                    if (message.protocolMessage.protocolMsgType == messages_pb2.ProtocolMessageType.CONNECT_MESSAGE):
                        print("    connectMessage")

                if not data:
                    break
                conn.sendall(data)

while True:
    Listen()
