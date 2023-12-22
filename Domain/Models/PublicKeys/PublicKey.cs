using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.PublicKeys;

public class PublicKey : Entity
{
    private PublicKey(Guid id) : base(id) { }

    public string Key { get; set; } = null!;
    public string InspectorId { get; set; } = null!;
    public ApplicationUser Inspector { get; set; } = null!;
    public List<Complaint> Complaints { get; set; } = new List<Complaint>();
    public bool IsDeleted { get; set; }

    public static PublicKey Create(string key, string inspectorId)
    {
        var publicKey = new PublicKey(Guid.NewGuid());
        publicKey.Key = key;
        publicKey.InspectorId = inspectorId;
        publicKey.IsDeleted = false;
        return publicKey;
    }
}
