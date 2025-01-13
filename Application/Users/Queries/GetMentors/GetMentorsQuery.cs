using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetMentors;

public record GetMentorsQuery(PagingInfo PagingInfo) 
    : IRequest<Result<PagedList<ApplicationUser>>>;