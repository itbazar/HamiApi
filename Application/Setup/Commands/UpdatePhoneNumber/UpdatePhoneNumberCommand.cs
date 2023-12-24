using MediatR;

namespace Application.Setup.Commands.UpdatePhoneNumber;

public record UpdatePhoneNumberCommand(string UserId, string NewPhoneNumber) : IRequest<bool>;