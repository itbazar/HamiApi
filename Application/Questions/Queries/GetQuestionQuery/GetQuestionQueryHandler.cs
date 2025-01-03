using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetQuestionQuery;

internal class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, Result<PagedList<Question>>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Result<PagedList<Question>>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false,
            false,
            s => s.OrderByDescending(o => o.TestType));
        return question;
    }
}