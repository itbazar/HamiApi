using Domain.Models.Hami;
using MediatR;

namespace Application.AnswerApp.Queries.GetAnswerByIdQuery;

public record GetAnswerByIdQuery(Guid Id) : IRequest<Result<Answer>>;
