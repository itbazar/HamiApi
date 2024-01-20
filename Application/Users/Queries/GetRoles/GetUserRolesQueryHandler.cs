using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetRoles;

internal class GetUserRolesQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserRolesQuery, Result<List<IsInRoleModel>>>
{
    public async Task<Result<List<IsInRoleModel>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRolesResult = await userRepository.GetUserRoles(request.UserId);
        if (userRolesResult.IsFailed)
            return userRolesResult.ToResult();
        var userRoles = userRolesResult.Value;
        var result = (await userRepository.GetRoles())
            .Select(role => new IsInRoleModel(role.Name ?? "", role.Title,
                role.Name != null && userRoles.Contains(role.Name)))
            .ToList();
        result.RemoveAll(p => p.RoleName == "PowerUser");

        return result;
    }
}
