using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetRoles;

internal class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<IsInRoleModel>>
{
    private readonly IUserRepository _userRepository;

    public GetUserRolesQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<IsInRoleModel>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _userRepository.GetUserRoles(request.UserId);
        var result = (await _userRepository.GetRoles())
            .Select(role => new IsInRoleModel(role.Name ?? "", role.Title,
                role.Name != null && userRoles.Contains(role.Name)))
            .ToList();
        result.RemoveAll(p => p.RoleName == "PowerUser");

        return result;
    }
}
