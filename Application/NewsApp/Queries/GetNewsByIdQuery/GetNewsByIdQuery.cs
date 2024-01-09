using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsByIdQuery;

public record GetNewsByIdQuery(Guid Id) : IRequest<News>;
