namespace Application.Common.Interfaces.Encryption;

public interface IAsymmetricEncryption
{
    byte[] EncryptAsymmetricByPublic(string publicKey, byte[] content);
    byte[] DecryptAsymmetricByPrivate(string privateKey, byte[] cipher);
    AsymmetricKey Generate();
}

public record AsymmetricKey(string PublicKey, string PrivateKey);