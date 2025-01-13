using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetPatients;

public record GetPatientsQuery(PagingInfo PagingInfo, RegistrationStatus? Status,string CurrentUserId) 
    : IRequest<Result<PagedList<ApplicationUser>>>;