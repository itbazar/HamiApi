using System.Security.Cryptography;

namespace Infrastructure.Encryption;

public class SymmetricEncryption
{
    public static byte[] Encrypt(SymmetricKeyComponents keyComponents, byte[] data)
    {
        return Encrypt(keyComponents.Key, keyComponents.IV, data);
    }

    public static byte[] Decrypt(SymmetricKeyComponents keyComponents, byte[] cipher)
    {
        return Decrypt(keyComponents.Key, keyComponents.IV, cipher);
    }

    public static byte[] Encrypt(byte[] key, byte[] iv, byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        return aes.EncryptCbc(data, iv, PaddingMode.PKCS7);
    }

    public static byte[] Decrypt(byte[] key, byte[] iv, byte[] cipher)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        return aes.DecryptCbc(cipher, iv, PaddingMode.PKCS7);
    }

    public static void GenerateKey(out byte[] key, out byte[] iv)
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

public class SymmetricKeyComponents
{
    const int citizenPasswordLength = 6;
    public SymmetricKeyComponents()
    {
        SymmetricEncryption.GenerateKey(out key, out iv);
    }

    public SymmetricKeyComponents(string passwordCitizen, string passwordServer, byte[] iv)
    {
        key = new byte[32];
        Convert.FromBase64String(passwordCitizen).CopyTo(key, 0);
        Convert.FromBase64String(passwordServer).CopyTo(key, citizenPasswordLength);
        this.iv = iv;
    }

    public SymmetricKeyComponents(byte[] key, byte[] iv)
    {
        this.key = key;
        this.iv = iv;
    }

    private byte[] key;
    private byte[] iv;
    public byte[] Key { get { return key; } }
    public byte[] IV { get { return iv; } }
    public string PasswordCitizen { get { return Convert.ToBase64String(Key[..citizenPasswordLength]); } }
    public string PasswordServer { get { return Convert.ToBase64String(Key[(citizenPasswordLength)..]); } }
}