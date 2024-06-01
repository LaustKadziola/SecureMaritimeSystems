using MMSAgent;
using Mmtp;

namespace AgentTest;

public class Tests
{
    Agent agent1 = new Agent("host1");

    [SetUp]
    public void Setup()
    {
        agent1.Verbose = true;
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Directory.CreateDirectory("Certificates");
        Utils.MakeCert("host1");
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
        agent1.ConnectAuthenticated("r1", "cart");
        long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds();
        Assert.That(agent1.Send(time, new List<string> { "host2" }, "hello"), Is.EqualTo(ResponseEnum.Good.ToString()));
    }

    [Test]
    public void TestSendRecieveOwn()
    {
        agent1.ConnectAuthenticated("r1", "cart");
        long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds();
        agent1.Send(time, new List<string> { "host1" }, "hello");
        string response = agent1.Receive();
        Assert.That(response, Is.EqualTo("hello"));
    }

    [Test]
    public void TestReconnectWithoutToken()
    {
        Assert.That(agent1.ConnectAuthenticated("r1", "cart"), Is.EqualTo(ResponseEnum.Good.ToString()));
        Assert.That(agent1.Disconnect(), Is.EqualTo(ResponseEnum.Good.ToString()));
        Assert.That(agent1.ConnectAuthenticated("r1", "cart"), Is.EqualTo(ResponseEnum.Good.ToString()));
    }


}