namespace Application.Communications.Commands;

public record AddSignalRConnectionIdCommand(string UserId, string ConnectionId) 
    : IRequest<Result<bool>>;
