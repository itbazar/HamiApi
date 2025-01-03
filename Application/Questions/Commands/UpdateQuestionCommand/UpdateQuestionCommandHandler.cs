using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.Questions.Commands.UpdateQuestionCommand;

internal class UpdateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateQuestionCommand, Result<Question>>
{
    public async Task<Result<Question>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetSingleAsync(s => s.Id == request.Id);
        if (question is null)
            throw new Exception("Not found.");
        question.Update(
            request.TestType,
            request.QuestionText);

        questionRepository.Update(question);
        await unitOfWork.SaveAsync();
        return question;
    }
}