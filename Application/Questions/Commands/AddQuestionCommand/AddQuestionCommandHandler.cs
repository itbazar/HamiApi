using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Commands.AddQuestionCommand;

internal class AddQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddQuestionCommand, Result<Question>>
{
    public async Task<Result<Question>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = Question.Create(
            request.TestType,
            request.QuestionText); 
        questionRepository.Insert(question);
        await unitOfWork.SaveAsync();
        return question;
    }
}