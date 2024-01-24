using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

public record GetAdminNewsQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<News>>>;
