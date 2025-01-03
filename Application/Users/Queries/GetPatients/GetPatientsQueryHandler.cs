using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetPatients;

internal class GetPatientsQueryHandler(IUserRepository userRepository) : IRequestHandler<GetPatientsQuery, Result<PagedList<ApplicationUser>>>
{
    public async Task<Result<PagedList<ApplicationUser>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetPagedPatientsAsync(request.PagingInfo);
        return result;
    }
}
