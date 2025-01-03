using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;

namespace Application.Common.Interfaces.Persistence;

public interface IUserGroupMembershipRepository
{
    public Task<Result<bool>> Insert(UserGroupMembership info);

    public Task<Result<bool>> Update(UserGroupMembership info);
}
