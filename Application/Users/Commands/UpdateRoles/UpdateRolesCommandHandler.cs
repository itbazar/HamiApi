using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Users.Commands.UpdateRoles;

internal class UpdateRolesCommandHandler : IRequestHandler<UpdateRolesCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateRolesCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        var roles = request.Roles.Where(role => role.IsIn).Select(role => role.RoleName)
            .ToList();
        if(roles is null)
            throw new Exception();
        var result = await _userRepository.UpdateRolesAsync(request.UserId, roles);
        return result;
    }
}
