using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

public record GetAdminNewsQuery() : IRequest<Result<List<News>>>;
