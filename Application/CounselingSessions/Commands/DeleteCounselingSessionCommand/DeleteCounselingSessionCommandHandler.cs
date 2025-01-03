using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Commands.DeleteCounselingSessionCommand;

internal class DeleteCounselingSessionCommandHandler(
    ICounselingSessionRepository counselingSessionrRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCounselingSessionCommand, Result<CounselingSession>>
{
    public async Task<Result<CounselingSession>> Handle(DeleteCounselingSessionCommand request, CancellationToken cancellationToken)
    {
        var counselingSessionr = await counselingSessionrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (counselingSessionr is null)
            return GenericErrors.NotFound;
        counselingSessionr.Delete(request.IsDeleted.Value);
        counselingSessionrRepository.Update(counselingSessionr);
        await unitOfWork.SaveAsync();
        return counselingSessionr;
    }
}