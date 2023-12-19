using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery(PagingInfo PagingInfo, int? InstanceId = null) : IRequest<PagedList<ApplicationUser>>;