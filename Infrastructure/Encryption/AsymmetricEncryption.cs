using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encryption;

public class AsymmetricEncryption
{
    public static byte[]? EncryptAsymmetric(string publicKey, byte[] content)
    {
        byte[]? result = null;
        try
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            result = rsa.Encrypt(content, RSAEncryptionPadding.Pkcs1);
        }
        catch(Exception e)
        {
            throw;
        }

        return result;
    }
}
