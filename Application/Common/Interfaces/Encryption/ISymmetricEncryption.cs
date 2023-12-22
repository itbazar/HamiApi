namespace Application.Common.Interfaces.Encryption;

public interface ISymmetricEncryption
{
    byte[] Decrypt(byte[] key, byte[] iv, byte[] cipher);
    byte[] Decrypt(SymmetricKeyComponents keyComponents, byte[] cipher);
    byte[] Encrypt(byte[] key, byte[] iv, byte[] data);
    byte[] Encrypt(SymmetricKeyComponents keyComponents, byte[] data);
    void GenerateKey(out byte[] key, out byte[] iv);
}