using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Answers.Commands.DeleteAnswerCommand;

internal class DeleteAnswerCommandHandler(
    IAnswerRepository answerrRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteAnswerCommand, Result<Answer>>
{
    public async Task<Result<Answer>> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answerr = await answerrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (answerr is null)
            return GenericErrors.NotFound;
        answerr.Delete(request.IsDeleted.Value);
        answerrRepository.Update(answerr);
        await unitOfWork.SaveAsync();
        return answerr;
    }
}