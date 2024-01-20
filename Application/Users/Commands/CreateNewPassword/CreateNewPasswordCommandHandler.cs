using Application.Common.Interfaces.Persistence;

namespace Application.Users.Commands.CreateNewPassword;

internal class CreateNewPasswordCommandHandler(IUserRepository userRepository) 
    : IRequestHandler<CreateNewPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateNewPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.CreateNewPasswordAsync(request.UserId, request.Password);
        if (!result)
            return UserErrors.PasswordUpdateFailed;
        return result;
    }
}
