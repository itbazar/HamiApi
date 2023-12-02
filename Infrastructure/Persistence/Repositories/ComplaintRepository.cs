using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;
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
    public bool Add(Complaint complaint)
    {
        complaint.TrackingNumber = GenerateTrackingNumber();
        complaint.PlainPassword = GeneratePassword();
        complaint.Password = PasswordHasher.HashPasword(complaint.PlainPassword);
        if (complaint.Contents.Count != 1)
            throw new Exception("Inconsistent contents.");
        EncryptComplaintContent(complaint, complaint.Contents[0]);

        _context.Add(complaint);
        return true;
    }

    public bool EncryptComplaintContent(Complaint complaint, ComplaintContent content)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var symKey = complaint.TrackingNumber + complaint.PlainPassword;
        var textBytes = unicodeEncoding.GetBytes(content.Text);
        if(textBytes is null)
        {
            throw new Exception("Invalid content.");
        }
        complaint.ContentsCitizen.Add(new ComplaintContentCitizen(Guid.NewGuid())
        {
            DateTime = DateTime.UtcNow,
            IsEncrypted = true,
            SymmetricCipher = SymmetricEncryption.EncryptString(symKey, textBytes) ?? throw new Exception("Symmetric encryption error."),
            MediaCitizen = new List<Domain.Models.Common.MediaCitizen>(),
            Sender = Actor.Citizen,
        });
        complaint.ContentsInspector.Add(new ComplaintContentInspector(Guid.NewGuid()) 
        { 
            DateTime = DateTime.UtcNow,
            IsEncrypted = true,
            AsymmetricCipher = AsymmetricEncryption.EncryptAsymmetric(_publicKey, textBytes) ?? throw new Exception("Asymmetric encryption errot."),
            Sender = Actor.Citizen
        });
        return true;
    }

    public bool DecryptComplaintContent(Complaint complaint)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var symKey = complaint.TrackingNumber + complaint.PlainPassword;

        foreach(var content in complaint.ContentsCitizen)
        {
            var textBytes = SymmetricEncryption.DecryptString(symKey, content.SymmetricCipher) ?? throw new Exception("Symmetric encryption error.");
            var text = unicodeEncoding.GetString(textBytes);
            complaint.Contents.Add(ComplaintContent.Create(content.Id, text));
        }
        
        return true;
    }
    public async Task<Complaint> GetAsync(string trackingNumber, string password, Actor actor)
    {
        var includeStr = actor == Actor.Inspector ? "ContentsInspector" : "ContentsCitizen";
        var complaint = await _context.Complaint
            .Where(c => c.TrackingNumber == trackingNumber)
            .Include(includeStr)
            .SingleOrDefaultAsync();
        if (complaint is null)
            throw new Exception("Not found");

        if (!PasswordHasher.VerifyPassword(password, complaint.Password))
            throw new Exception("Wrong password");

        complaint.PlainPassword = password;
        DecryptComplaintContent(complaint);
        return complaint;
    }

    private string GenerateTrackingNumber()
    {
        return (Random.Shared.NextInt64() % 1000000).ToString();
    }

    private string GeneratePassword()
    {
        return (Random.Shared.NextInt64() % 10000).ToString();
    }
}
