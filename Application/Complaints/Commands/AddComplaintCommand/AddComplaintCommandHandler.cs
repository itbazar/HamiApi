using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler : IRequestHandler<AddComplaintCommand, AddComplaintResult>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddComplaintCommandHandler(IComplaintRepository complaintRepository, IUnitOfWork unitOfWork)
    {
        _complaintRepository = complaintRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddComplaintResult> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {
        var complaint = Complaint.Register(request.Title, request.Text, request.CategoryId);
        _complaintRepository.Add(complaint);
        await _unitOfWork.SaveAsync();
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
