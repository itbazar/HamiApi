namespace Api.Contracts.NewsContract;

public record UpdateNewsDto(
    string? Title,
    IFormFile? Image,
    string? Url,
    string? Description,
    string? Content);
