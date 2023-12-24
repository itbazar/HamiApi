using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Setup.Commands.UpdatePhoneNumber;

internal sealed class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdatePhoneNumberCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            throw new Exception("Not found!");
        user.PhoneNumber = request.NewPhoneNumber;
        await _userRepository.Update(user);
        
        return true;
    }
}