using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.Answers.Commands.UpdateAnswerCommand;

internal class UpdateAnswerCommandHandler(
    IAnswerRepository answerRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateAnswerCommand, Result<Answer>>
{
    public async Task<Result<Answer>> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await answerRepository.GetSingleAsync(s => s.Id == request.Id);
        if (answer is null)
            throw new Exception("Not found.");
        answer.Update(request.AnswerValue);

        answerRepository.Update(answer);
        await unitOfWork.SaveAsync();
        return answer;
    }
}