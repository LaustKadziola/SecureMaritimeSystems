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
    public static void Main()
    {
        Console.WriteLine("starting test");
        int n = 1000;

        List<Agent> agents = [];

        for (int i = 0; i < n; i++)
        {
            string mrn = $"stress host{i}";
            Console.WriteLine($"Initailizing {mrn}");

            Agent agent = new(mrn)
            {
                Verbose = false
            };

            Utils.MakeCert(mrn);
            string certificate = GenerateCertificate(mrn);
            agent.ConnectAuthenticated("r1", certificate);
            long time = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5)).ToUnixTimeMilliseconds();
            agent.Send(time, [mrn], "message");
            agents.Add(agent);
        }

        Console.WriteLine("Test Initailized");
        while (true)
        {
            foreach (Agent agent in agents)
            {
                agent.Receive();
            }
            Thread.Sleep(100);
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