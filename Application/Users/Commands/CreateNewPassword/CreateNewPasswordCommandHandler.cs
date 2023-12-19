using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Users.Commands.CreateNewPassword;

internal class CreateNewPasswordCommandHandler : IRequestHandler<CreateNewPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public CreateNewPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> Handle(CreateNewPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = _userRepository.CreateNewPasswordAsync(request.UserId, request.Password);
        return result;
    }
}
