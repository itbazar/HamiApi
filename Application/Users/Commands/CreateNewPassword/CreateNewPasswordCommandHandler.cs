using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Users.Commands.CreateNewPassword;

internal class CreateNewPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateNewPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateNewPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.CreateNewPasswordAsync(request.UserId, request.Password);
        return result;
    }
}
