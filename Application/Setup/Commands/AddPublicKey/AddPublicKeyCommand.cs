using MediatR;

namespace Application.Setup.Commands.AddPublicKey;

public sealed record AddPublicKeyCommand(string Title, string publicKey) : IRequest<Result<bool>>;
