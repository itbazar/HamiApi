using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetRoles;

public record GetUserRolesQuery(string UserId):IRequest<List<IsInRoleModel>>;
