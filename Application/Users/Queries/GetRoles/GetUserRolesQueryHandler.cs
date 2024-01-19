using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetRoles;

internal class GetUserRolesQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserRolesQuery, Result<List<IsInRoleModel>>>
{
    public async Task<Result<List<IsInRoleModel>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await userRepository.GetUserRoles(request.UserId);
        var result = (await userRepository.GetRoles())
            .Select(role => new IsInRoleModel(role.Name ?? "", role.Title,
                role.Name != null && userRoles.Contains(role.Name)))
            .ToList();
        result.RemoveAll(p => p.RoleName == "PowerUser");

        return result;
    }
}
