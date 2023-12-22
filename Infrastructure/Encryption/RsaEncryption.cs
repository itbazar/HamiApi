using Application.Common.Interfaces.Encryption;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encryption;

public class RsaEncryption : IAsymmetricEncryption
{
    public byte[] EncryptAsymmetricByPublic(string publicKey, byte[] content)
    {
        var base64Key = Convert.ToBase64String(content);
        var base64Bytes = Encoding.UTF8.GetBytes(base64Key);
        byte[] result;
        try
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(publicKey);
            result = rsa.Encrypt(base64Bytes, RSAEncryptionPadding.Pkcs1);
        }
        catch
        {
            throw;
        }
        return result;
    }

    public AsymmetricKey Generate()
    {
        var rsa = new RSACryptoServiceProvider();
        var pub = rsa.ExportRSAPublicKeyPem();
        var pri = rsa.ExportRSAPrivateKeyPem();
        return new AsymmetricKey(pub, pri);
    }

    public byte[] DecryptAsymmetricByPrivate(string privateKey, byte[] cipher)
    {
        byte[] resultBase64;
        try
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(privateKey);
            resultBase64 = rsa.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);
        }
        catch
        {
            throw;
        }
        var base64Key = Encoding.UTF8.GetString(resultBase64);
        var result = Convert.FromBase64String(base64Key);

        return result;
    }
}

