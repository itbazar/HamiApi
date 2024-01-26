namespace Domain.Models.ComplaintAggregate.Encryption;

public class SymmetricKeyComponents
{
    const int citizenPasswordLength = 6;
    public SymmetricKeyComponents(ISymmetricEncryption symmetricEncryption)
    {
        symmetricEncryption.GenerateKey(out key, out iv);
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
    public string PasswordServer { get { return Convert.ToBase64String(Key[citizenPasswordLength..]); } }
}
