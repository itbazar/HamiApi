using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class ReplyComplaintInspectorCommandHandler(IComplaintRepository complaintRepository) : IRequestHandler<ReplyComplaintInspectorCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ReplyComplaintInspectorCommand request, CancellationToken cancellationToken)
    {
        var result = await complaintRepository.GetAsync(request.TrackingNumber);
        if (result.IsFailed)
            return result.ToResult();
        var complaint = result.Value;
        try
        {
            complaint.AddContent(
            request.Text,
            request.Medias.Adapt<List<Media>>(),
            Actor.Inspector,
            request.Operation,
            request.IsPublic ? ComplaintContentVisibility.Everyone : ComplaintContentVisibility.Inspector);
        }
        catch
        {
            return ComplaintErrors.InvalidOperation;
        }
        
        complaint.ReplyInspector(request.EncodedKey);

        await complaintRepository.Update(complaint);
        return true;
    }
}
