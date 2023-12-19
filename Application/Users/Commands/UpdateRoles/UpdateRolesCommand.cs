using Application.Users.Common;
using MediatR;

namespace Application.Users.Commands.UpdateRoles;

public record UpdateRolesCommand(string UserId, List<IsInRoleModel> Roles):IRequest<bool>;
