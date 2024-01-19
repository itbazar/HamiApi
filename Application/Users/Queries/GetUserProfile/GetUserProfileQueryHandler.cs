using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileQuery, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.FindByIdAsync(request.UserId);
        if (result == null)
        {
            return UserErrors.UserNotExsists;
        }

        return result;
    }
}
