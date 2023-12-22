namespace Application.Common.Interfaces.Encryption;

public interface IAsymmetricEncryption
{
    byte[] DecryptAsymmetric(string publicKey, byte[] cipher);
    byte[] EncryptAsymmetric(string publicKey, byte[] content);
    AsymmetricKey Generate();
}

public record AsymmetricKey(string PublicKey, string PrivateKey);