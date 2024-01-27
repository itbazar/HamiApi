using Domain.Models.ComplaintAggregate.Encryption;

namespace Application.Setup.Commands.GenerateKeyPair;

public record GenerateKeyPairCommand : IRequest<Result<AsymmetricKey>>;
