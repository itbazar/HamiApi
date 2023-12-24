using Application.Common.Interfaces.Persistence;
using Domain.Models.PublicKeys;
using MediatR;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed class ChangeInspectorKeyCommandHandler : IRequestHandler<ChangeInspectorKeyCommand, bool>
{
    private readonly IComplaintRepository _complaintRepository;

    public ChangeInspectorKeyCommandHandler(
        IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<bool> Handle(ChangeInspectorKeyCommand request, CancellationToken cancellationToken)
    {
        return await _complaintRepository.ChangeInspectorKey(request.PrivateKey, request.ToKeyId, request.FromKeyId);
    }
}
