using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class ReplyComplaintInspectorCommandHandler : IRequestHandler<ReplyComplaintInspectorCommand, bool>
{
    private readonly IComplaintRepository _complaintRepository;
    public ReplyComplaintInspectorCommandHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<bool> Handle(ReplyComplaintInspectorCommand request, CancellationToken cancellationToken)
    {
        var complaint = await _complaintRepository.GetAsync(request.TrackingNumber);
        complaint.AddContent(
            request.Text,
            request.Medias.Adapt<List<Media>>(),
            Actor.Inspector,
            request.Operation,
            request.IsPublic ? ComplaintContentVisibility.Everyone : ComplaintContentVisibility.Inspector);
        await _complaintRepository.ReplyInspector(complaint, request.EncodedKey);
        return true;
    }
}
