using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.ReplyComplaintCitizenCommand;

public class ReplyComplaintCitizenCommandHandler : IRequestHandler<ReplyComplaintCitizenCommand, bool>
{
    private readonly IComplaintRepository _complaintRepository;
    public ReplyComplaintCitizenCommandHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<bool> Handle(ReplyComplaintCitizenCommand request, CancellationToken cancellationToken)
    {
        var complaint = await _complaintRepository.GetAsync(request.TrackingNumber);
        complaint.AddContent(
            request.Text,
            request.Medias.Adapt<List<Media>>(),
            Actor.Citizen,
            request.Operation,
            ComplaintContentVisibility.Everyone);
        await _complaintRepository.ReplyCitizen(complaint, request.Password);
        return true;
    }
}
