﻿using System.Security.Cryptography;
using System.Text;

namespace Domain.Models.ComplaintAggregate.Encryption;

public class Hasher : IHasher
{
    const int keySize = 64;
    const int iterations = 350000;
    static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public Password HashPasword(string password)
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

    public bool VerifyPassword(string password, Password passwordHash)
    {
        var salt = passwordHash.Salt;
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, passwordHash.Hash);
    }

    public byte[] Hash(byte[] data)
    {
        return SHA512.HashData(data);
    }

    public bool Verify(byte[] data, byte[] hash)
    {
        return SHA512.HashData(data).SequenceEqual(hash);
    }
}
