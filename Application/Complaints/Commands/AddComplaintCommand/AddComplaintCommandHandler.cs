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

public class AddComplaintCommandHandler : IRequestHandler<AddComplaintCommand, AddComplaintResult>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IPublicKeyRepository _publicKeyRepository;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ICommunicationService _communicationService;
    private readonly IUserRepository _userRepository;
    public AddComplaintCommandHandler(
        IComplaintRepository complaintRepository,
        IPublicKeyRepository publicKeyRepository,
        ICaptchaProvider captchaProvider,
        ICommunicationService communicatorService,
        IUserRepository userRepository)
    {
        _complaintRepository = complaintRepository;
        _publicKeyRepository = publicKeyRepository;
        _captchaProvider = captchaProvider;
        _communicationService = communicatorService;
        _userRepository = userRepository;
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
        var publicKey = (await _publicKeyRepository.GetAll())
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

        await _complaintRepository.Add(complaint);
        if(complaint.UserId is not null)
        {
            var user = await _userRepository.FindByIdAsync(complaint.UserId);
            if (user?.PhoneNumber is null)
                throw new Exception("User not found.");
            var messages = complaint.GetMessages(ComplaintOperation.Register);
            await messages.SendMessages(_communicationService, user, null);
        }
        return new AddComplaintResult(complaint.TrackingNumber, complaint.PlainPassword);
    }
}
