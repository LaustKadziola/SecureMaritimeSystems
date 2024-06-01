using MMSAgent;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Security;


class RunAgent
{

    static bool isRunning = true;


    public static void Main(string[] args)
    {
        string mrn = args[0];

        Agent agent = new(mrn);
        agent.Verbose = true;
        string certificate = GenerateCertificate(mrn);

        Utils.MakeCert(mrn);

        while (isRunning)
        {
            Console.Write("Input Command: ");
            string str = Console.ReadLine() ?? string.Empty;
            string response = str switch
            {
                "c" => agent.ConnectAuthenticated("r1", certificate),
                "s" => SendHelper(agent),
                "q" => Quit(),
                "r" => agent.Receive(),
                "d" => agent.Disconnect(),
                _ => "Undefined Command"
            };

            Console.WriteLine(response);
        }

    }

    private static string Quit()
    {
        isRunning = false;
        return "quiting";
    }

    private static string SendHelper(Agent agent)
    {
        Console.Write("Message: ");
        string message = Console.ReadLine() ?? string.Empty;
        Console.Write("MRN: ");
        List<string> mrn = [Console.ReadLine() ?? "undefined"];
        Console.WriteLine($"{DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds()}");
        Console.WriteLine($"{(int)DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds()}");
        return agent.Send(DateTimeOffset.Now.Add(TimeSpan.FromMinutes(0.3)).ToUnixTimeMilliseconds(), mrn, message);
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