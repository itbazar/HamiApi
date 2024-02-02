using Application.Common.Interfaces.Persistence;
using Mapster;
using MediatR;

namespace Application.Setup.Queries.GetPublicKeys;

public sealed class GetPublicKeysQueryHandler : IRequestHandler<GetPublicKeysQuery, Result<List<PublicKeyResponse>>>
{
    private readonly IPublicKeyRepository _publicKeyRepository;

    public GetPublicKeysQueryHandler(IPublicKeyRepository publicKeyRepository)
    {
        _publicKeyRepository = publicKeyRepository;
    }

    public async Task<Result<List<PublicKeyResponse>>> Handle(GetPublicKeysQuery request, CancellationToken cancellationToken)
    {
        var result = await _publicKeyRepository.GetAll();
        if (result.IsFailed)
            return result.ToResult();
        return result.Value.Adapt<List<PublicKeyResponse>>();
    }
}
