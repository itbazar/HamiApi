using Application.Common.Interfaces.Encryption;
using MediatR;

namespace Application.Setup.Commands.GenerateKeyPair;

public record GenerateKeyPairCommand : IRequest<Result<AsymmetricKey>>;
