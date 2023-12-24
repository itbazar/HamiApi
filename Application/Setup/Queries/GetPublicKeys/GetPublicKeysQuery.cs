using MediatR;

namespace Application.Setup.Queries.GetPublicKeys;

public sealed record GetPublicKeysQuery() : IRequest<List<PublicKeyResponse>>;
public record PublicKeyResponse(Guid id, string Title, string InspectorId, bool IsActive);