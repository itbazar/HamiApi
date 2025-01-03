﻿using Domain.Models.ComplaintAggregate;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.PublicKeys;

public class PublicKey : Entity
{
    private PublicKey(Guid id) : base(id) { }
    public string Title { get; set; } = string.Empty;
    public string Key { get; set; } = null!;
    public string InspectorId { get; set; } = null!;
    public ApplicationUser Inspector { get; set; } = null!;
    public List<Complaint> Complaints { get; set; } = new List<Complaint>();
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public static PublicKey Create(string title, string key, string inspectorId, bool isActive = false)
    {
        var publicKey = new PublicKey(Guid.NewGuid());
        publicKey.Title = title;
        publicKey.Key = key;
        publicKey.InspectorId = inspectorId;
        publicKey.IsActive = isActive;
        return publicKey;
    }

    public static AsymmetricKey GenerateKeyPair()
    {
        IAsymmetricEncryption asymmetricEncryption = new RsaEncryption();
        return asymmetricEncryption.Generate();
    }
}
