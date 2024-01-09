using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Commands.DeleteNewsCommand;

public record DeleteNewsCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<News>;
