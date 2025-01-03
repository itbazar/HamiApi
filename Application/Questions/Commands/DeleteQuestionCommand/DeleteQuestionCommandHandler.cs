using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Commands.DeleteQuestionCommand;

internal class DeleteQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteQuestionCommand, Result<Question>>
{
    public async Task<Result<Question>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetSingleAsync(s => s.Id == request.Id);
        if (question is null)
            return GenericErrors.NotFound;
        question.Delete(request.IsDeleted.Value);
        questionRepository.Update(question);
        await unitOfWork.SaveAsync();
        return question;
    }
}