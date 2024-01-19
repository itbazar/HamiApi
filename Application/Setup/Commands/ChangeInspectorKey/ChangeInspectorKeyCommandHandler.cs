using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed class ChangeInspectorKeyCommandHandler(
    IComplaintRepository complaintRepository) : IRequestHandler<ChangeInspectorKeyCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeInspectorKeyCommand request, CancellationToken cancellationToken)
    {
        return await complaintRepository.ChangeInspectorKey(request.PrivateKey, request.ToKeyId, request.FromKeyId);
    }
}
