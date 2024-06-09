
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace MMSAgent;

public static class Utils
{
    public static string GetpathPrivate(string mrn)
    {
        return Path.Combine(
        Directory.GetCurrentDirectory(),
        "Certificates", $"private-{mrn}.prm");
    }

    public static string GetpathPublic(string mrn)
    {
        return Path.Combine(
        Directory.GetCurrentDirectory(),
        "Certificates", $"Public-{mrn}.prm");
    }

    public static void DeletePublicKey(string mrn)
    {
        File.Delete(GetpathPublic(mrn));
    }

    public static void KeyPair(string mrn)
    {
        if (File.Exists(GetpathPrivate(mrn)) && File.Exists(GetpathPublic(mrn)))
        {
            return;
        }

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

        PemWriter pemWriterPrivate = new PemWriter(new StreamWriter(GetpathPrivate(mrn)));
        pemWriterPrivate.WriteObject(privateKey);
        pemWriterPrivate.Writer.Flush();
        pemWriterPrivate.Writer.Close();

        PemWriter pemWriterPublic = new PemWriter(new StreamWriter(GetpathPublic(mrn)));
        pemWriterPublic.WriteObject(publicKey);
        pemWriterPublic.Writer.Flush();
        pemWriterPublic.Writer.Close();
    }
}