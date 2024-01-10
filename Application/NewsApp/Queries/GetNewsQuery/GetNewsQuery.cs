using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsQuery;

public record GetNewsQuery() : IRequest<List<News>>;
