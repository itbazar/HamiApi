using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.QuestionApp.Queries.GetQuestionByIdQuery;

internal class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Result<Question>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Result<Question>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (question is null)
            return GenericErrors.NotFound;
        return question;
    }
}