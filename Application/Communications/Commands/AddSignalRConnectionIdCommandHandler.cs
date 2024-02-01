
using Application.Common.Interfaces.Communication;

namespace Application.Communications.Commands;

internal class AddSignalRConnectionIdCommandHandler(ICommunicationService communicationService)
        : IRequestHandler<AddSignalRConnectionIdCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddSignalRConnectionIdCommand request, CancellationToken cancellationToken)
    {
        await communicationService.AddCommunicationId(request.UserId, request.ConnectionId);
        return true;
    }
}
