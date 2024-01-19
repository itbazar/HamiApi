using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result<ApplicationUser>>
{
    public async Task<Result<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
        };
        var result = await userRepository.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new Exception();
        return user;
    }
}
