using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encryption;

public class SymmetricEncryption
{
    public static byte[]? EncryptString(string key, byte[] content)
    {
        key = key.PadLeft(16).Substring(0, 16);
        byte[] iv = new byte[16];

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;

        return aes.EncryptCbc(content, iv);
    }

    public static byte[]? DecryptString(string key, byte[] cipher)
    {
        key = key.PadLeft(16).Substring(0, 16);
        byte[] iv = new byte[16];

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;

        return aes.DecryptCbc(cipher, iv);
    }
}
