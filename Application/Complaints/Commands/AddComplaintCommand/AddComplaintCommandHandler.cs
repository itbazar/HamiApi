using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler : IRequestHandler<AddComplaintCommand, AddComplaintResult>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IPublicKeyRepository _publicKeyRepository;
    public AddComplaintCommandHandler(IComplaintRepository complaintRepository, IPublicKeyRepository publicKeyRepository)
    {
        _complaintRepository = complaintRepository;
        _publicKeyRepository = publicKeyRepository;
    }

    public async Task<AddComplaintResult> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {
        var publicKey = (await _publicKeyRepository.GetAll()).Where(p => p.IsDeleted == false).FirstOrDefault();
        if (publicKey is null)
            throw new Exception("There is no active public key.");

        var complaint = Complaint.Register(
            publicKey,
            request.Title,
            request.Text,
            request.CategoryId,
            request.Medias.Adapt<List<Media>>());

        await _complaintRepository.Add(complaint);
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
