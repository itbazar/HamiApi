using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encryption;

public class AsymmetricEncryption
{
    public static byte[] EncryptAsymmetric(string publicKey, byte[] content)
    {
        var base64Key = Convert.ToBase64String(content);
        var base64Bytes = Encoding.UTF8.GetBytes(base64Key);
        byte[] result;
        try
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            result = rsa.Encrypt(base64Bytes, RSAEncryptionPadding.Pkcs1);
        }
        catch(Exception e)
        {
            throw;
        }
        var resultBase64 = Convert.ToBase64String(result);
        return result;
    }

    public static byte[] DecryptAsymmetric(string publicKey, byte[] cipher)
    {
        byte[] resultBase64;
        try
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            resultBase64 = rsa.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);
        }
        catch (Exception e)
        {
            throw;
        }
        var base64Key = Encoding.UTF8.GetString(resultBase64);
        var result = Convert.FromBase64String(base64Key);

        return result;
    }

}
