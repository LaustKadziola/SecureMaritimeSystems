
using System.Diagnostics;
using MMSAgent;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

class RunPerformanceTest
{

    public static void Main()
    {
        Stopwatch sw = new Stopwatch();
        Console.WriteLine("Starting Preformance Tests");

        Console.WriteLine("- Connection stest");
        int nConnectionTests = 500;

        sw.Start();
        for (int i = 0; i < nConnectionTests; i++)
        {
            string mrn = $"connTest-{i}";
            Agent agent = new Agent(mrn);

            agent.ConnectAuthenticated("r1", "cert");
            agent.Disconnect();
        }
        sw.Stop();
        Console.WriteLine($"Average time {(double)sw.ElapsedMilliseconds / nConnectionTests}");
        Console.WriteLine("");


        // Here starts Send / receive tests
        Console.WriteLine("- Send/receiveTests starting");
        int nsrTests = 500;
        string mrn1 = $"sendTest";
        Agent agent1 = new Agent(mrn1);
        Utils.KeyPair("sendTest");
        agent1.ConnectAuthenticated("r1", "cert");
        sw.Restart();
        for (int i = 0; i < nsrTests; i++)
        {
            long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5)).ToUnixTimeMilliseconds();
            agent1.Send(time, [mrn1], "Test Message");
            string m = agent1.Receive()[1];

            if (m != "Test Message")
                Console.WriteLine("Failed");
        }
        agent1.Disconnect();
        sw.Stop();
        Console.WriteLine($"Average time {(double)sw.ElapsedMilliseconds / nsrTests}");
    }
}