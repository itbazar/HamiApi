using Application.Common.Errors;
using Application.Common.Interfaces.Encryption;
using MediatR;

namespace Application.Setup.Commands.GenerateKeyPair;

internal class GenerateKeyPairCommandHandler(IAsymmetricEncryption asymmetricEncryption) : IRequestHandler<GenerateKeyPairCommand, Result<AsymmetricKey>>
{
    public async Task<Result<AsymmetricKey>> Handle(GenerateKeyPairCommand request, CancellationToken cancellationToken)
    {
        var result = asymmetricEncryption.Generate();
        if (result is null)
            return EncryptionErrors.KeyGenerationFailed;
        await Task.CompletedTask;
        return result;
    }
}
