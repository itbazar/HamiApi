using Domain.Models.News;

namespace Application.NewsApp.Queries.GetAdminNewsByIdQuery;

public record GetAdminNewsByIdQuery(Guid Id) : IRequest<Result<News>>;
