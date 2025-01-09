using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.AnswerApp.Queries.GetAnswerByIdQuery;

internal class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, Result<Answer>>
{
    private readonly IAnswerRepository _answerRepository;

    public GetAnswerByIdQueryHandler(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<Result<Answer>> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        var answer = await _answerRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (answer is null)
            return GenericErrors.NotFound;
        return answer;
    }
}