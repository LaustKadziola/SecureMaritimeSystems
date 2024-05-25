using System.Net;

namespace MMSEdgeRouter;

class Run
{
    public static void Main()
    {
        // Console.WriteLine(Dns.GetHostName());

        // foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        // {
        //     Console.WriteLine(address.ToString());
        // }

        EdgeRouter edgeRouter = new("10.0.1.1", 65432);
        edgeRouter.Run();
    }
}