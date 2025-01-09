using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Answers.Queries.GetAnswerQuery;

public record GetAnswerQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<Answer>>>;
