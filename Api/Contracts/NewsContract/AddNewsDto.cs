namespace Api.Contracts.NewsContract;

public record AddNewsDto(
    string Title,
    IFormFile Image,
    string Url = "",
    string Description = "",
    string Content = "");