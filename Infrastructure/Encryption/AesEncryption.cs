using Application.Common.Interfaces.Encryption;
using System.Security.Cryptography;

namespace Infrastructure.Encryption;

public class AesEncryption : ISymmetricEncryption
{
    public byte[] Encrypt(SymmetricKeyComponents keyComponents, byte[] data)
    {
        return Encrypt(keyComponents.Key, keyComponents.IV, data);
    }

    public byte[] Decrypt(SymmetricKeyComponents keyComponents, byte[] cipher)
    {
        return Decrypt(keyComponents.Key, keyComponents.IV, cipher);
    }

    public byte[] Encrypt(byte[] key, byte[] iv, byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        return aes.EncryptCbc(data, iv, PaddingMode.PKCS7);
    }

    public byte[] Decrypt(byte[] key, byte[] iv, byte[] cipher)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        return aes.DecryptCbc(cipher, iv, PaddingMode.PKCS7);
    }

    public void GenerateKey(out byte[] key, out byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();
        key = aes.Key;
        iv = aes.IV;
        /*
        var rnd = new System.Security.Cryptography.RandomNumberGenerator.Create();
        var key = new byte[50];
        rnd.GetBytes(key);
        */
    }
}
