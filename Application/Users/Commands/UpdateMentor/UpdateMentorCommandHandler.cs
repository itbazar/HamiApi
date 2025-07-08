using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.UpdateMentor;

internal class UpdateMentorCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateMentorCommand, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(UpdateMentorCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.MentorId);
        if (user is null)
            return UserErrors.UserNotExsists;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Title = request.Title ?? user.Title;
        user.Email = request.Email ?? user.Email;
        //user.DateOfBirth = request.DateOfBirth ?? user.DateOfBirth;
        user.Gender = request.Gender ?? user.Gender;
        user.Education = request.Education ?? user.Education;
        user.City = request.City ?? user.City;

        await userRepository.Update(user);

        return user;
    }
}
