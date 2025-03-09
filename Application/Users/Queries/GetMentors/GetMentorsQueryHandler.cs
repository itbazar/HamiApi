using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetMentors;

internal class GetMentorsQueryHandler(IUserRepository userRepository) : IRequestHandler<GetMentorsQuery, Result<PagedList<ApplicationUser>>>
{
    public async Task<Result<PagedList<ApplicationUser>>> Handle(GetMentorsQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository
            .GetPagedMentorsAsync(request.PagingInfo);
        return result;
    
    }
}
