using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Answers.Commands.AddAnswerCommand;

internal class AddAnswerCommandHandler(
    IAnswerRepository answerRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddAnswerCommand, Result<Answer>>
{
    public async Task<Result<Answer>> Handle(AddAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = Answer.Create(
            request.UserId,
            request.QuestionId,
            request.AnswerValue,
            request.TestPeriodId
        ); 

        answerRepository.Insert(answer);
        await unitOfWork.SaveAsync();
        return answer;
    }
}