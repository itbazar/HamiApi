using Domain.Models.ComplaintAggregate;

namespace Application.Common.Interfaces.Encryption;

public interface IHasher
{
    byte[] Hash(byte[] data);
    Password HashPasword(string password);
    bool Verify(byte[] data, byte[] hash);
    bool VerifyPassword(string password, Password passwordHash);
}