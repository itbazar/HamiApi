using Application.Common.Interfaces.Encryption;
using Domain.Models.ComplaintAggregate;
using System.Text;

namespace Infrastructure.ExtensionMethods;

public static class ComplaintContentExtensionMethods
{
    public static bool Encrypt(
        this ComplaintContent content,
        byte[] key,
        byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var textBytes = unicodeEncoding.GetBytes(content.Text);
        content.Cipher = symmetricEncryption.Encrypt(key, iv, textBytes) ?? throw new Exception("Symmetric encryption error.");
        content.IntegrityHash = hasher.Hash(textBytes);
        content.IsEncrypted = true;

        foreach (var media in content.Media)
        {
            media.Cipher = symmetricEncryption.Encrypt(key, iv, media.Data);
            media.IntegrityHash = hasher.Hash(media.Data);
        }
        return true;
    }

    public static bool Decrypt(this ComplaintContent content, byte[] key, byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        if (content.Cipher is null)
            throw new Exception("Cipher is null");
        var textBytes = symmetricEncryption.Decrypt(key, iv, content.Cipher) ?? throw new Exception("Symmetric encryption error.");
        if (!hasher.Verify(textBytes, content.IntegrityHash))
            throw new Exception("Invalid hash");
        content.Text = unicodeEncoding.GetString(textBytes);
        content.IsEncrypted = false;

        foreach (var media in content.Media)
        {
            media.Data = symmetricEncryption.Decrypt(key, iv, media.Cipher);
            if (!hasher.Verify(media.Data, media.IntegrityHash))
                throw new Exception("Invalid hash");
        }
        return true;
    }
}
