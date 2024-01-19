using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserById;

internal class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.UserNotExsists;
        return user;
    }
}
