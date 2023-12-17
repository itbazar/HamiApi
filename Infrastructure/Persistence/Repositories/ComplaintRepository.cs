﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Infrastructure.Persistence.Repositories;

public class ComplaintRepository : IComplaintRepository
{
    private ApplicationDbContext _context;
    private string _publicKey;
    public ComplaintRepository(ApplicationDbContext context)
    {
        _context = context;
        _publicKey = @"<RSAKeyValue><Modulus>jZ5bPPhJCpFRMQV1zUzZ8uR6tO9OHM1n9z6+7Xb8lBO+s7FABC9SpfHI9nCE7BYCyhV8pQd7uRrLrouKERX0vVsn8Lc285XVbFlEiqoqAo6qNfda7pFMQ8j6afvoTgDBSndFhiKc5ZX53K5dFK0iP4xZ7PD7l/Zjh5wAs1S+MYk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    }
    public async Task<bool> Add(Complaint complaint)
    {
        complaint.TrackingNumber = GenerateTrackingNumber();

        complaint.GenerateCredentials(_publicKey);
        
        if (complaint.Contents.Count != 1)
            throw new Exception("Inconsistent contents.");

        complaint.EncryptContent();

        _context.Add(complaint);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Complaint> GetCitizenAsync(string trackingNumber, string password)
    {
        var complaint = await _context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .Include(c => c.Contents)
            .ThenInclude(c => c.Media)
            .SingleOrDefaultAsync();

        if (complaint is null)
            throw new Exception("Not found");

        complaint.LoadEncryptionKeyByCitizenPassword(password);
        complaint.DecryptContent();
        return complaint;
    }

    public async Task<Complaint> GetInspectorAsync(string trackingNumber, string encodedKey)
    {
        var complaint = await _context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .Include(c => c.Contents)
            .ThenInclude(c => c.Media)
            .SingleOrDefaultAsync();

        if (complaint is null)
            throw new Exception("Not found");

        complaint.LoadEncryptionKeyByInspector(encodedKey);
        complaint.DecryptContent();
        return complaint;
    }

    public async Task<Complaint> GetAsync(string trackingNumber)
    {
        var complaint = await _context.Complaint.Where(c => c.TrackingNumber == trackingNumber).SingleOrDefaultAsync();
        return complaint ?? throw new Exception("Not found.");
    }

    public async Task<List<Complaint>> GetListAsync(PagingInfo pagingInfo)
    {
        var complaintList = await _context.Complaint
            .Skip(pagingInfo.PageSize * (pagingInfo.PageNumber -1))
            .Take(pagingInfo.PageSize)
            .ToListAsync();
        return complaintList;
    }

    public async Task<bool> ReplyInspector(Complaint complaint, string encodedKey)
    {
        complaint.LoadEncryptionKeyByInspector(encodedKey);
        complaint.EncryptContent();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReplyCitizen(Complaint complaint, string password)
    {
        complaint.LoadEncryptionKeyByCitizenPassword(password);
        complaint.EncryptContent();
        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateTrackingNumber()
    {
        return RandomNumberGenerator.GetInt32(10000000, 99999999).ToString();
    }
}


public static class ComplaintExtensionMethods
{
    public static bool GenerateCredentials(this Complaint complaint, string _publicKey)
    {
        // Generate citizen key and password
        SymmetricKeyComponents encryptionKeyCitizen = new SymmetricKeyComponents();
        complaint.ServerPassword = encryptionKeyCitizen.PasswordServer;
        complaint.PlainPassword = encryptionKeyCitizen.PasswordCitizen;
        complaint.CitizenPassword = Hasher.HashPasword(complaint.PlainPassword);
        complaint.EncryptionIvCitizen = encryptionKeyCitizen.IV;

        //Generate report encryption key
        SymmetricKeyComponents encryptionKey = new SymmetricKeyComponents();
        complaint.EncryptionKey = encryptionKey.Key;
        complaint.EncryptionIv = encryptionKey.IV;
        complaint.EncryptionKeyPassword = Hasher.HashPasword(complaint.EncryptionKey.ToBase64());

        // Store report encryption key as encrypted by citizen and inspector keys
        complaint.CipherKeyInspector = AsymmetricEncryption.EncryptAsymmetric(_publicKey, complaint.EncryptionKey);
        complaint.CipherKeyCitizen = SymmetricEncryption.Encrypt(encryptionKeyCitizen, encryptionKey.Key);

        return true;
    }
    public static bool LoadEncryptionKeyByCitizenPassword(this Complaint complaint, string password)
    {
        if (!Hasher.VerifyPassword(password, complaint.CitizenPassword))
            throw new Exception("Wrong password");
        complaint.PlainPassword = password;
        var citizenEncryptionKey = new SymmetricKeyComponents(password, complaint.ServerPassword, complaint.EncryptionIvCitizen);
        complaint.EncryptionKey = SymmetricEncryption.Decrypt(citizenEncryptionKey, complaint.CipherKeyCitizen);
        if (!Hasher.VerifyPassword(complaint.EncryptionKey.ToBase64(), complaint.EncryptionKeyPassword))
            throw new Exception("Invalid encryption key");
        return true;
    }

    public static bool LoadEncryptionKeyByInspector(this Complaint complaint, string encodedKey)
    {
        complaint.EncryptionKey = encodedKey.ParseBytes();
        if (!Hasher.VerifyPassword(complaint.EncryptionKey.ToBase64(), complaint.EncryptionKeyPassword))
            throw new Exception("Invalid encryption key");
        return true;
    }

    public static bool EncryptContent(this Complaint complaint, int index = 0) 
    {
        if (complaint.EncryptionKey is null || complaint.EncryptionIv is null)
            throw new Exception("Encryption key or iv is null");
        complaint.Contents[index].Encrypt(complaint.EncryptionKey, complaint.EncryptionIv);
        return true;
    }
    public static bool DecryptContent(this Complaint complaint)
    {
        if (complaint.EncryptionKey is null || complaint.EncryptionIv is null)
            throw new Exception("Encryption key or iv is null");
        foreach (var content in complaint.Contents)
        {
            content.Decrypt(complaint.EncryptionKey, complaint.EncryptionIv);
        }

        return true;
    }
}
public static class ComplaintContentExtensionMethods
{
    public static bool Encrypt(this ComplaintContent content, byte[] key, byte[] iv)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var textBytes = unicodeEncoding.GetBytes(content.Text);
        content.Cipher = SymmetricEncryption.Encrypt(key, iv, textBytes) ?? throw new Exception("Symmetric encryption error.");
        content.IntegrityHash = Hasher.Hash(textBytes);
        content.IsEncrypted = true;

        foreach(var media in content.Media)
        {
            media.Cipher = SymmetricEncryption.Encrypt(key, iv, media.Data);
            media.IntegrityHash = Hasher.Hash(media.Data);
        }
        return true;
    }

    public static bool Decrypt(this ComplaintContent content, byte[] key, byte[] iv)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        if (content.Cipher is null)
            throw new Exception("Cipher is null");
        var textBytes = SymmetricEncryption.Decrypt(key, iv, content.Cipher) ?? throw new Exception("Symmetric encryption error.");
        if (!Hasher.Verify(textBytes, content.IntegrityHash))
            throw new Exception("Invalid hash");
        content.Text = unicodeEncoding.GetString(textBytes);
        content.IsEncrypted = false;

        foreach (var media in content.Media)
        {
            media.Data = SymmetricEncryption.Decrypt(key, iv, media.Cipher);
            if (!Hasher.Verify(media.Data, media.IntegrityHash))
                throw new Exception("Invalid hash");
        }
        return true;
    }
}
public static class ByteArrayExtensionMethods
{
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }
    public static byte[] ParseBytes(this string base64String)
    {
        return Convert.FromBase64String(base64String);
    }
}