using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;

internal class GetSessionUsersQueryHandler(ICounselingSessionRepository counselingSessionRepository,
    IUserGroupMembershipRepository userGroupMembershipRepository) : IRequestHandler<GetSessionUsersQuery, Result<List<ApplicationUser>>>
{
    public async Task<Result<List<ApplicationUser>>> Handle(GetSessionUsersQuery request, CancellationToken cancellationToken)
    {
        var session = await counselingSessionRepository
            .GetAsync(q => q.Id == request.SessionId && !q.IsDeleted);
        if (session is null || session.FirstOrDefault() is null)
        {
            return GenericErrors.NotFound;
        }

        var userMemberships = await userGroupMembershipRepository.GetAsync(
           q => q.PatientGroupId == session.FirstOrDefault().PatientGroupId && !q.IsDeleted,
           includeProperties: "User");

        var users = userMemberships.Select(um => um.User).ToList();

        return users;
    }
}