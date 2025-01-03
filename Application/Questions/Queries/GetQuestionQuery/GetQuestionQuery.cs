using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetQuestionQuery;

public record GetQuestionQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<Question>>>;
