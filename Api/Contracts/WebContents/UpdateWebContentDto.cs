namespace Api.Contracts.WebContents;

public record UpdateWebContentDto(
    string? Title,
    string? Description,
    string? Content);
