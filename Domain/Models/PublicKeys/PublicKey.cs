using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.PublicKeys;

public class PublicKey : Entity
{
    private PublicKey(Guid id) : base(id) { }

    public string Key { get; set; } = null!;
    public string InspectorId { get; set; } = null!;
    public ApplicationUser Inspector { get; set; } = null!;
}
