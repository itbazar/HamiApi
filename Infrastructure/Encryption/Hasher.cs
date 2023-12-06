using Domain.Models.ComplaintAggregate;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encryption;

public class Hasher
{
    const int keySize = 64;
    const int iterations = 350000;
    static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public static Password HashPasword(string password)
    {
        var result = new Password();
        var salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        result.Hash = hash;
        result.Salt = salt;
        return result;
    }

    public static bool VerifyPassword(string password, Password passwordHash)
    {
        var salt = passwordHash.Salt;
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, passwordHash.Hash);
    }

    public static byte[] Hash(byte[] data)
    {
        return SHA512.HashData(data);
    }

    public static bool Verify(byte[] data, byte[] hash)
    {
        return SHA512.HashData(data).SequenceEqual(hash);
    }
}
