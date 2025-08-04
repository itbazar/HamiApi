using Domain.Models.Hami;
using MediatR;

namespace Application.CounselingSessions.Commands.DeleteCounselingSessionCommand;

public record DeleteCounselingSessionCommand(
    Guid Id,
    bool? IsDeleted = true) : IRequest<Result<CounselingSession>>;
