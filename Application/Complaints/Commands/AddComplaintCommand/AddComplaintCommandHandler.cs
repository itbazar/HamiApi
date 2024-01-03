using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler : IRequestHandler<AddComplaintCommand, AddComplaintResult>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IPublicKeyRepository _publicKeyRepository;
    private readonly ICaptchaProvider _captchaProvider;
    public AddComplaintCommandHandler(IComplaintRepository complaintRepository, IPublicKeyRepository publicKeyRepository, ICaptchaProvider captchaProvider)
    {
        _complaintRepository = complaintRepository;
        _publicKeyRepository = publicKeyRepository;
        _captchaProvider = captchaProvider;
    }

    public async Task<AddComplaintResult> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }
        var publicKey = (await _publicKeyRepository.GetAll()).Where(p => p.IsActive == true).FirstOrDefault();
        if (publicKey is null)
            throw new Exception("There is no active public key.");

        var complaint = Complaint.Register(
            request.UserId,
            publicKey,
            request.Title,
            request.Text,
            request.CategoryId,
            request.Medias.Adapt<List<Media>>(),
            request.Complaining,
            request.OrganizationId);

        await _complaintRepository.Add(complaint);
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
