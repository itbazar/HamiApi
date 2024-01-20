using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;

namespace Application.Complaints.Commands.ReplyComplaintCitizenCommand;

public class ReplyComplaintCitizenCommandHandler(IComplaintRepository complaintRepository) : IRequestHandler<ReplyComplaintCitizenCommand, Result<bool>>
{
public async Task<Result<bool>> Handle(ReplyComplaintCitizenCommand request, CancellationToken cancellationToken)
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
            Actor.Citizen,
            request.Operation,
            ComplaintContentVisibility.Everyone);
        }
        catch
        {
            return ComplaintErrors.InvalidOperation;
        }
        
        await complaintRepository.ReplyCitizen(complaint, request.Password);
        return true;
    }
}
