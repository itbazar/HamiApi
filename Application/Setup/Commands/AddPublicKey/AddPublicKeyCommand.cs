using MediatR;

namespace Application.Setup.Commands.AddPublicKey;

public sealed record AddPublicKeyCommand(string publicKey) : IRequest<bool>;
