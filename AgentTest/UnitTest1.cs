using MMSAgent;
using Mmtp;

namespace AgentTest;

public class Tests
{
    Agent agent1 = new Agent("host1");

    [SetUp]
    public void Setup()
    {
        agent1.Verbose = false;
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Directory.CreateDirectory("Certificates");
        Utils.KeyPair("host1");
    }

    [TearDown]
    public void TearDown()
    {
        agent1.Disconnect();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void TestConnect()
    {
        string response = agent1.ConnectAuthenticated("r1", "cert");
        Assert.That(response, Is.EqualTo(ResponseEnum.Good.ToString()));
    }

    [Test]
    public void TestSend()
    {
        agent1.ConnectAuthenticated("r1", "cert");
        long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds();
        Assert.That(agent1.Send(time, new List<string> { "host2" }, "hello"), Is.EqualTo(ResponseEnum.Good.ToString()));
    }

    [Test]
    public void TestSendRecieve()
    {
        agent1.Verbose = true;
        agent1.ConnectAuthenticated("r1", "cert");
        long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds();
        agent1.Send(time, new List<string> { "host1" }, "hello");
        List<string> response = agent1.Receive();
        Assert.That(response[0], Is.EqualTo(ResponseEnum.Good.ToString()));
        Assert.That(response[1], Is.EqualTo("hello"), "recived message was hello");
    }

    [Test]
    public void TestReconnect()
    {
        Assert.That(agent1.ConnectAuthenticated("r1", "cert"), Is.EqualTo(ResponseEnum.Good.ToString()));
        Assert.That(agent1.Disconnect(), Is.EqualTo(ResponseEnum.Good.ToString()));
        Assert.That(agent1.ConnectAuthenticated("r1", "cert"), Is.EqualTo(ResponseEnum.Good.ToString()));
    }

    [Test]
    public void TestNoKey()
    {
        Agent agentNoKey = new Agent("hostNoKey");
        agentNoKey.Verbose = true;
        Utils.KeyPair("hostNoKey");
        Utils.DeletePublicKey("hostNoKey");


        agentNoKey.ConnectAuthenticated("r1", "cert");
        agent1.ConnectAuthenticated("r1", "cert");

        long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds();
        agentNoKey.Send(time, new List<string> { "host1" }, "This Shoud Not Be Verified");

        List<string> response = agent1.Receive();

        Assert.That(response[0], Is.EqualTo(ResponseEnum.Good.ToString()), "Status of empty message should be good");
        Assert.That(response.Count, Is.EqualTo(1));
    }


}