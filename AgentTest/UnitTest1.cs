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
}