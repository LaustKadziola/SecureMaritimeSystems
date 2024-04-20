#Server

import socket

HOST = "10.0.0.1"
PORT = 65432

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()
    with conn:
        (ip_addr, port) = addr
        print("Connected by " + str(ip_addr) + ", " + str(port))
        while True:
            data = conn.recv(1024)
            if not data:
                break
            conn.sendall(data)
