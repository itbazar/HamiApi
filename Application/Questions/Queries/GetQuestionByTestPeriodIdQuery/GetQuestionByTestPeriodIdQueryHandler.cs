using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetQuestionByTestPeriodIdQuery;

internal class GetQuestionByTestPeriodIdQueryHandler(
    IQuestionRepository questionRepository,
    ITestPeriodRepository testPeriodRepository) : IRequestHandler<GetQuestionByTestPeriodIdQuery, Result<List<Question>>>
{
    public async Task<Result<List<Question>>> Handle(GetQuestionByTestPeriodIdQuery request, CancellationToken cancellationToken)
    {
        // یافتن دوره آزمون
        var testPeriod = await testPeriodRepository.GetSingleAsync(tp => tp.Id == request.TestPeriodId && !tp.IsDeleted);
        if (testPeriod == null)
            return TestPeriodErrors.NotFound;

        // گرفتن سوالات آزمون
        var questions = await questionRepository.GetAsync(q => q.TestType == testPeriod.TestType && !q.IsDeleted);
        return questions.ToList();
    }
}