using MediatR;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed record ChangeInspectorKeyCommand(
    string PrivateKey,
    Guid ToKeyId,
    Guid? FromKeyId = null) : IRequest<Result<bool>>;
