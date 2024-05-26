using MMSAgent;
using Mmtp;

namespace AgentTest;

public class Tests
{
    Agent agent1 = new Agent("host1");

    [SetUp]
    public void Setup()
    {
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
    public void TestSendRecieveOwn()
    {
        agent1.ConnectAuthenticated("r1", "cart");
        agent1.Send(0, new List<string> { "host1" }, "hello");
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