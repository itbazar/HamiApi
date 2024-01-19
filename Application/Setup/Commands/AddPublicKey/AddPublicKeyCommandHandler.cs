using Application.Common.Interfaces.Persistence;
using Domain.Models.PublicKeys;
using MediatR;

namespace Application.Setup.Commands.AddPublicKey;

public sealed class AddPublicKeyCommandHandler(IPublicKeyRepository publicKeyRepository, IUserRepository userRepository) : IRequestHandler<AddPublicKeyCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddPublicKeyCommand request, CancellationToken cancellationToken)
    {
        var inspector = await userRepository.GetUsersInRole("Inspector");
        if (inspector is null || inspector.Count() == 0)
        {
            throw new Exception("No inspector found.");
        }
        var publicKey = PublicKey.Create(request.Title, request.publicKey, inspector.First().Id);
        await publicKeyRepository.Add(publicKey);
        return true;
    }
}
