using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsQuery;

public record GetNewssQuery() : IRequest<List<News>>;
