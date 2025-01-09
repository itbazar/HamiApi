using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Answers.Queries.GetAnswerQuery;

internal class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, Result<PagedList<Answer>>>
{
    private readonly IAnswerRepository _answerRepository;

    public GetAnswerQueryHandler(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<Result<PagedList<Answer>>> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
    {
        var answer = await _answerRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false,
            false,
            s => s.OrderByDescending(o => o.TestPeriodId));
        return answer;
    }
}