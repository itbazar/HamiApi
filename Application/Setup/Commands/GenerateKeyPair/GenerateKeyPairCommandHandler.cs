using Application.Common.Interfaces.Encryption;
using MediatR;

namespace Application.Setup.Commands.GenerateKeyPair;

internal class GenerateKeyPairCommandHandler : IRequestHandler<GenerateKeyPairCommand, AsymmetricKey>
{
    private readonly IAsymmetricEncryption _asymmetricEncryption;

    public GenerateKeyPairCommandHandler(IAsymmetricEncryption asymmetricEncryption)
    {
        _asymmetricEncryption = asymmetricEncryption;
    }

    public async Task<AsymmetricKey> Handle(GenerateKeyPairCommand request, CancellationToken cancellationToken)
    {
        var result = _asymmetricEncryption.Generate();
        if (result is null)
            throw new Exception();
        await Task.CompletedTask;
        return result;
    }
}
