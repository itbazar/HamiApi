using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetQuestionByTestPeriodIdQuery;

public record GetQuestionByTestPeriodIdQuery(Guid TestPeriodId) : IRequest<Result<List<Question>>>;
