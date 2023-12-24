using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ApplicationUser>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindByIdAsync(request.UserId);
        if (result == null)
        {
            throw new Exception("Not found.");
        }

        return result;
    }
}
