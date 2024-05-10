#Server

import socket
import json
import uuid
import messages_pb2
import threading

HOST = "10.0.1.1"
PORT = 65432

connections = dict()
subscriptions = dict()

messageBuffer = dict()

def ConnectionToAgent(conn, addr):
    mrn = ""
    with conn:
        (ip_addr, port) = addr
        print("Connected by " + str(ip_addr) + ", " + str(port))

        while True:
            data = conn.recv(1024)
            if data:
                message = messages_pb2.MmtpMessage()
                message.ParseFromString(data)

                if(message.msgType == messages_pb2.MsgType.PROTOCOL_MESSAGE):
                    print("ProtocolMessage")
                    if (message.protocolMessage.protocolMsgType == messages_pb2.ProtocolMessageType.CONNECT_MESSAGE):
                        mrn = message.protocolMessage.connectMessage.ownMrn
                        print("    connectMessage")
                        print(mrn)
                        print(message.protocolMessage.connectMessage.reconnectToken)
                        AddConnection(message, ip_addr, port)
                        conn.sendall(data)
                    elif (message.protocolMessage.protocolMsgType == messages_pb2.ProtocolMessageType.SEND_MESSAGE):
                        print("    Send Message")
                        ReceivedSendMessage(message)
                        conn.sendall(data)
                    elif (message.protocolMessage.protocolMsgType == messages_pb2.ProtocolMessageType.RECIEVE_MESSAGE):
                        print("    recieve Message")
                        print(message.protocolMessage.connectMessage.ownMrn)
                        print(message.protocolMessage.connectMessage.reconnectToken)
                        HandelReceiveMessage(message, mrn, conn)


    pass


def StartUp():
    pass

def Send(MRN, subject):
    pass

def Notify_from(From_Agent):
    pass

def Notify_to(To_Agent):
    pass


def AddConnection(message, ip, port):
    mrn = message.protocolMessage.connectMessage.ownMrn
    connections[mrn] = (ip, port)

    print(connections[mrn])
    pass

def ReceivedSendMessage(message):
    applicationMessage = message.protocolMessage.sendMessage.applicationMessage
    body = applicationMessage.body.decode('utf-8')
    length = applicationMessage.header.bodySizeNumBytes
    recipients = applicationMessage.header.recipients.recipients
    print(body)
    print("length = " + str(length))

    for recipient in recipients:
        print(recipient)
        if (not recipient in messageBuffer):
            messageBuffer[recipient] = []
        messageBuffer[recipient].append(message)

    pass

def HandelReceiveMessage(message, mrn, conn):
    receiveMessage = message.protocolMessage.receiveMessage

    messagesBuffer = messageBuffer[mrn]

    response = messages_pb2.MmtpMessage()
    response.msgType = messages_pb2.REDPONSE_MESSAGE
    response.uuid = str(uuid.uuid4())

    responseMessage = response.prsponseMessage
    responseMessage.responseToUuid = message.uuid
    responseMessage.response = messages_pb2.ResponseEnum.GOOD

    for m in messagesBuffer:
        print(m)
        applicationMessage = m.protocolMessage.sendMessage.applicationMessage
        responseMessage.applicationMessage.extend([applicationMessage])

    print(response)

    message_str = response.SerializeToString()

    print(message_str)

    conn.sendall(message_str)

    pass


def Listen(s):
    print("listening at: " + HOST + ", " + str(PORT))
    s.listen()
    conn, addr = s.accept()
    threading.Thread(target=ConnectionToAgent, args=(conn, addr)).start()

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    while True:
        Listen(s)
