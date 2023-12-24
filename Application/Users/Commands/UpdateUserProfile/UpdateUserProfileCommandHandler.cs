using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ApplicationUser>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            throw new Exception("Not found.");
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Title = request.Title ?? user.Title;
        user.NationalId = request.NationalId ?? user.NationalId;
        user.PhoneNumber2 = request.PhoneNumber2 ?? user.PhoneNumber2;

        await _userRepository.Update(user);

        return user;
    }
}
