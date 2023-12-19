using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
        };
        var result = await _userRepository.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new Exception();
        return user;
    }
}
