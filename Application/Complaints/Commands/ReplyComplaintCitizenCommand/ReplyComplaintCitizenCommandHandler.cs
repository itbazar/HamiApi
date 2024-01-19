using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.ReplyComplaintCitizenCommand;

public class ReplyComplaintCitizenCommandHandler(IComplaintRepository complaintRepository) : IRequestHandler<ReplyComplaintCitizenCommand, Result<bool>>
{
public async Task<Result<bool>> Handle(ReplyComplaintCitizenCommand request, CancellationToken cancellationToken)
    {
        var complaint = await complaintRepository.GetAsync(request.TrackingNumber);
        complaint.AddContent(
            request.Text,
            request.Medias.Adapt<List<Media>>(),
            Actor.Citizen,
            request.Operation,
            ComplaintContentVisibility.Everyone);
        await complaintRepository.ReplyCitizen(complaint, request.Password);
        return true;
    }
}
