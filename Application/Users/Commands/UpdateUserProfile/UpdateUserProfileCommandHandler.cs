using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserProfileCommand, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.UserNotExsists;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Title = request.Title ?? user.Title;
        user.NationalId = request.NationalId ?? user.NationalId;
        user.PhoneNumber2 = request.PhoneNumber2 ?? user.PhoneNumber2;

        await userRepository.Update(user);

        return user;
    }
}
