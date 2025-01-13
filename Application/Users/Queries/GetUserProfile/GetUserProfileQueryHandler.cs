using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler(IUserRepository userRepository,
    IUserGroupMembershipRepository userGroupMembershipRepository,
    IPatientGroupRepository patientGroupRepository) : IRequestHandler<GetUserProfileQuery, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.FindByIdAsync(request.UserId);
        if (result == null)
        {
            return UserErrors.UserNotExsists;
        }

        if(request.mode == "Patient")
        {
            // پیدا کردن عضویت کاربر در گروه
            var userGroupMembership = await userGroupMembershipRepository.FindByUserIdAsync(request.UserId);
            if (userGroupMembership is null)
                return UserErrors.UserGroupNotAssigned;

            // پیدا کردن گروه بیمار
            var patientGroup = await patientGroupRepository.GetFirstAsync(q => q.Id == userGroupMembership.Value.PatientGroupId);
            if (patientGroup is null)
                return UserErrors.UserGroupNotAssigned;

            // پیدا کردن منتور
            var mentor = await userRepository.FindByIdAsync(patientGroup.MentorId);
            if (mentor is null)
                return UserErrors.UserNotExsists;

            patientGroup.Mentor = mentor;
            result.UserGroupMemberships.Add(userGroupMembership.Value);
            result.UserGroupMemberships.First().PatientGroup = patientGroup;
        }

        return result;
    }
}
