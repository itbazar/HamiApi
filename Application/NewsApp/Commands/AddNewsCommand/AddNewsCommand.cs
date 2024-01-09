using Domain.Models.News;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.AddNewsCommand;

public record AddNewsCommand(
    string Title,
    IFormFile Image,
    string Url,
    string Description,
    string Content) : IRequest<News>;
