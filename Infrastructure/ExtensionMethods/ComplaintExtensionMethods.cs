using Application.Common.Interfaces.Encryption;
using Domain.Models.ComplaintAggregate;

namespace Infrastructure.ExtensionMethods;

public static class ComplaintExtensionMethods
{
    public static bool GenerateCredentials(
        this Complaint complaint,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption,
        IAsymmetricEncryption asymmetricEncryption)
    {
        // Generate citizen key and password
        SymmetricKeyComponents encryptionKeyCitizen = new SymmetricKeyComponents(symmetricEncryption);
        complaint.ServerPassword = encryptionKeyCitizen.PasswordServer;
        complaint.PlainPassword = encryptionKeyCitizen.PasswordCitizen;
        complaint.CitizenPassword = hasher.HashPasword(complaint.PlainPassword);
        complaint.EncryptionIvCitizen = encryptionKeyCitizen.IV;

        //Generate report encryption key
        SymmetricKeyComponents encryptionKey = new SymmetricKeyComponents(symmetricEncryption);
        complaint.EncryptionKey = encryptionKey.Key;
        complaint.EncryptionIv = encryptionKey.IV;
        complaint.EncryptionKeyPassword = hasher.HashPasword(complaint.EncryptionKey.ToBase64());

        // Store report encryption key as encrypted by citizen and inspector keys
        complaint.CipherKeyInspector = asymmetricEncryption.EncryptAsymmetricByPublic(complaint.PublicKey.Key, complaint.EncryptionKey);
        complaint.CipherKeyCitizen = symmetricEncryption.Encrypt(encryptionKeyCitizen, encryptionKey.Key);

        return true;
    }

    public static bool LoadEncryptionKeyByCitizenPassword(
        this Complaint complaint,
        string password,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        if (!hasher.VerifyPassword(password, complaint.CitizenPassword))
            throw new Exception("Wrong password");
        complaint.PlainPassword = password;
        var citizenEncryptionKey = new SymmetricKeyComponents(password, complaint.ServerPassword, complaint.EncryptionIvCitizen);
        complaint.EncryptionKey = symmetricEncryption.Decrypt(citizenEncryptionKey, complaint.CipherKeyCitizen);
        if (!hasher.VerifyPassword(complaint.EncryptionKey.ToBase64(), complaint.EncryptionKeyPassword))
            throw new Exception("Invalid encryption key");
        return true;
    }

    public static bool LoadEncryptionKeyByInspector(this Complaint complaint, string encodedKey, IHasher hasher)
    {
        complaint.EncryptionKey = encodedKey.ParseBytes();
        if (!hasher.VerifyPassword(complaint.EncryptionKey.ToBase64(), complaint.EncryptionKeyPassword))
            throw new Exception("Invalid encryption key");
        return true;
    }

    public static bool ChangeInspectorKey(
        this Complaint complaint,
        string fromPrivateKey,
        string toPublicKey,
        IHasher hasher,
        IAsymmetricEncryption asymmetricEncryption)
    {
        complaint.EncryptionKey = asymmetricEncryption.DecryptAsymmetricByPrivate(fromPrivateKey, complaint.CipherKeyInspector);
        if (!hasher.VerifyPassword(complaint.EncryptionKey.ToBase64(), complaint.EncryptionKeyPassword))
            throw new Exception("Invalid encryption key");
        complaint.CipherKeyInspector = asymmetricEncryption.EncryptAsymmetricByPublic(toPublicKey, complaint.EncryptionKey);
        return true;
    }

    public static bool EncryptContent(
        this Complaint complaint,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption,
        int index = 0)
    {
        if (complaint.EncryptionKey is null || complaint.EncryptionIv is null)
            throw new Exception("Encryption key or iv is null");
        complaint.Contents[index].Encrypt(complaint.EncryptionKey, complaint.EncryptionIv, hasher, symmetricEncryption);
        return true;
    }
    public static bool DecryptContent(this Complaint complaint,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        if (complaint.EncryptionKey is null || complaint.EncryptionIv is null)
            throw new Exception("Encryption key or iv is null");
        foreach (var content in complaint.Contents)
        {
            content.Decrypt(complaint.EncryptionKey, complaint.EncryptionIv, hasher, symmetricEncryption);
        }

        return true;
    }
}
