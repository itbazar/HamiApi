
using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.Hami.Events;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.ApprovedRegisterPatient;

public class ApprovedRegisterPatientCommandHandler(
    IUserRepository userRepository,IUserGroupMembershipRepository userGroupMembershipRepository) : IRequestHandler<ApprovedRegisterPatientCommand, Result<AddPatientResult>>
{
    public async Task<Result<AddPatientResult>> Handle(
        ApprovedRegisterPatientCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.UserNotExsists;

        if (user.RegistrationStatus != RegistrationStatus.Pending)
            return UserErrors.UserIsNotPending;

        if (request.IsApproved)
        {
            user.RegistrationStatus = RegistrationStatus.Approved;
            // اختصاص گروه به بیمار
            if (request.PatientGroupId.HasValue)
            {
                await userRepository.Update(user);
                var result = UserGroupMembership.Register(user.Id, request.PatientGroupId.Value, user.PhoneNumber);
                if (result.IsFailed)
                {
                    return result.ToResult();
                }
                var temp = result.Value;
                await userGroupMembershipRepository.Insert(temp);
            }
            else
                return UserErrors.UserGroupNotAssigned;
        }
        else
        {
            user.RegistrationStatus = RegistrationStatus.Rejected;
            user.RejectionReason = request.RejectionReason;
            await userRepository.Update(user);
        }
       

        return new AddPatientResult(user.UserName, user.PhoneNumber);
    }
}
