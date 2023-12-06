using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler : IRequestHandler<AddComplaintCommand, AddComplaintResult>
{
    private readonly IComplaintRepository _complaintRepository;
    
    public AddComplaintCommandHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<AddComplaintResult> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {
        var complaint = Complaint.Register(
            request.Title,
            request.Text,
            request.CategoryId,
            request.Medias.Adapt<List<Media>>());

        await _complaintRepository.Add(complaint);
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
