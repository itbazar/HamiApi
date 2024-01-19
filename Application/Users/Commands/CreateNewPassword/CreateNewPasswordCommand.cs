using MediatR;

namespace Application.Users.Commands.CreateNewPassword;

public record CreateNewPasswordCommand(
    string UserId,
    string Password) : IRequest<Result<bool>>;
