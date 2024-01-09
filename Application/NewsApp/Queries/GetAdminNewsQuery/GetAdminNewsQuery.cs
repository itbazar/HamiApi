using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

public record GetAdminNewsQuery() : IRequest<List<News>>;
