using Domain.Models.Hami;
using MediatR;

namespace Application.QuestionApp.Queries.GetQuestionByIdQuery;

public record GetQuestionByIdQuery(Guid Id) : IRequest<Result<Question>>;
