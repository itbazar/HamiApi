using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler(
    IComplaintRepository complaintRepository,
    IPublicKeyRepository publicKeyRepository,
    ICaptchaProvider captchaProvider) : IRequestHandler<AddComplaintCommand, Result<AddComplaintResult>>
{
public async Task<Result<AddComplaintResult>> Handle(
    AddComplaintCommand request,
    CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }
        var publicKeyResult = await publicKeyRepository.GetActive();
        if (publicKeyResult.IsFailed)
            return publicKeyResult.ToResult();
        var publicKey = publicKeyResult.Value;

        var registerResult = Complaint.Register(
            request.UserId,
            publicKey,
            request.Title,
            request.Text,
            request.CategoryId,
            request.Medias.Adapt<List<Media>>(),
            request.Complaining,
            request.OrganizationId);

        if (registerResult.IsFailed)
        {
            return registerResult.ToResult();
        }
        var complaint = registerResult.Value;
        await complaintRepository.Insert(complaint);
        
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
