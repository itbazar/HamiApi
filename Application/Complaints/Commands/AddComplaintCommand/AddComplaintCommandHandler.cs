using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.ExtensionMethods;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler(
    IComplaintRepository complaintRepository,
    IPublicKeyRepository publicKeyRepository,
    ICaptchaProvider captchaProvider,
    ICommunicationService communicatorService,
    IUserRepository userRepository) : IRequestHandler<AddComplaintCommand, Result<AddComplaintResult>>
{
public async Task<Result<AddComplaintResult>> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }
        var publicKey = (await publicKeyRepository.GetAll())
            .Where(p => p.IsActive == true).FirstOrDefault();
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

        await complaintRepository.Add(complaint);
        if(complaint.UserId is not null)
        {
            var user = await userRepository.FindByIdAsync(complaint.UserId);
            if (user?.PhoneNumber is null)
                return GenericErrors.NotFound;
            var messages = complaint.GetMessages(ComplaintOperation.Register);
            await messages.SendMessages(communicatorService, user, null);
        }
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
