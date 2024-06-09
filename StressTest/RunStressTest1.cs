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

class Program
{
    public static void Main(string[] args)
    {
        string prefix = string.Empty;
        if (args.Length > 0)
        {
            prefix = args[0];
        }
        Console.WriteLine("starting test");
        int n = 1000;

        List<Agent> agents = [];

        for (int i = 0; i < n; i++)
        {
            string mrn = $"{prefix}-stress-host{i}";
            Console.WriteLine($"Initailizing {mrn}");

            Agent agent = new(mrn)
            {
                Verbose = false
            };

            Utils.KeyPair(mrn);
            string certificate = GenerateCertificate(mrn);
            agent.ConnectAuthenticated("r1", certificate);
            long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5)).ToUnixTimeMilliseconds();
            agent.Send(time, [mrn], "message");
            agents.Add(agent);


        }

        foreach (Agent agent in agents)
        {
            Thread agnetThread = new Thread(() => RunAgnet(agent));
            agnetThread.Start();
        }

        Console.WriteLine("Test Initailized");

    }

    private static void RunAgnet(Agent agent)
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5)).ToUnixTimeMilliseconds();
                agent.Send(time, [agent.ownMrn], "message");
                agent.Receive();
            }
            agent.Disconnect();
            agent.ConnectAuthenticated("r1", "cert");

            //Thread.Sleep(100);
        }
    }

    private static string GenerateCertificate(string mrn)
    {
        var curvename = "secp256k1";

        X9ECParameters par = ECNamedCurveTable.GetByName(curvename);

        var curParam = new ECDomainParameters(par.Curve, par.G, par.N, par.H, par.GetSeed());

        ECKeyGenerationParameters keyGenParam = new ECKeyGenerationParameters(curParam, new SecureRandom());

        ECKeyPairGenerator generator = new ECKeyPairGenerator();

        generator.Init(keyGenParam);

        AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var certGenerator = new X509V3CertificateGenerator();

        certGenerator.SetSubjectDN(new X509Name($"CN={mrn}"));
        certGenerator.SetNotAfter(DateTime.UtcNow.AddDays(30));
        certGenerator.SetNotBefore(DateTime.UtcNow);
        certGenerator.SetPublicKey(publicKey);
        certGenerator.SetIssuerDN(new X509Name($"CN={mrn}"));
        certGenerator.SetSerialNumber(BigInteger.ValueOf(1));

        var algorithm = X9ObjectIdentifiers.ECDsaWithSha256.ToString();
        var cert = certGenerator.Generate(new Asn1SignatureFactory(algorithm, privateKey));

        string? certStr = Convert.ToBase64String(cert.GetEncoded());
        if (certStr is not null)
        {
            return certStr;
        }
        else
        {
            throw new Exception("Certificate failed to generate into string");
        }
    }
}