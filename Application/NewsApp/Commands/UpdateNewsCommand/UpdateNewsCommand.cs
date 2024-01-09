using Domain.Models.News;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.UpdateNewsCommand;

public record UpdateNewsCommand(
    Guid Id,
    string? Title,
    IFormFile? Image,
    string? Url,
    string? Description) : IRequest<News>;
