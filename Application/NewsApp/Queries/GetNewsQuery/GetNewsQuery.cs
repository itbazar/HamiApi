using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Application.NewsApp.Queries.GetNewsQuery;

public record GetNewsQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<News>>>;
