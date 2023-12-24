using Application.Common.Interfaces.Persistence;
using Domain.Models.PublicKeys;
using MediatR;

namespace Application.Setup.Commands.AddPublicKey;

public sealed class AddPublicKeyCommandHandler : IRequestHandler<AddPublicKeyCommand, bool>
{
    private readonly IPublicKeyRepository _publicKeyRepository;
    private readonly IUserRepository _userRepository;

    public AddPublicKeyCommandHandler(IPublicKeyRepository publicKeyRepository, IUserRepository userRepository)
    {
        _publicKeyRepository = publicKeyRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(AddPublicKeyCommand request, CancellationToken cancellationToken)
    {
        var inspector = await _userRepository.GetUsersInRole("Inspector");
        if (inspector is null || inspector.Count() == 0)
        {
            throw new Exception("No inspector found.");
        }
        var publicKey = PublicKey.Create(request.Title, request.publicKey, inspector.First().Id);
        await _publicKeyRepository.Add(publicKey);
        return true;
    }
}
