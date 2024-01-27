using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.PublicKeys;

namespace Application.Setup.Commands.GenerateKeyPair;

internal class GenerateKeyPairCommandHandler
    : IRequestHandler<GenerateKeyPairCommand, Result<AsymmetricKey>>
{
    public async Task<Result<AsymmetricKey>> Handle(GenerateKeyPairCommand request, CancellationToken cancellationToken)
    {
        var result = PublicKey.GenerateKeyPair();
        if (result is null)
            return EncryptionErrors.KeyGenerationFailed;
        await Task.CompletedTask;
        return result;
    }
}
