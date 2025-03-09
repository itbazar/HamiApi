using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IUserGroupMembershipRepository : IGenericRepository<UserGroupMembership>
{
    public Task<Result<bool>> Insert(UserGroupMembership info);

    public Task<Result<bool>> Update(UserGroupMembership info);

    public Task<Result<UserGroupMembership>> FindByUserIdAsync(string userId);
}
