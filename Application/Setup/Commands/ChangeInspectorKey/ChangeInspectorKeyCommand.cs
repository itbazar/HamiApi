using MediatR;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed record ChangeInspectorKeyCommand(
    string PrivateKey,
    Guid ToKeyId,
    bool IsPolling = false,
    Guid? FromKeyId = null) : IRequest<Result<ChangeInspectorKeyResponse>>;

public record ChangeInspectorKeyResponse(long Total, long Done);