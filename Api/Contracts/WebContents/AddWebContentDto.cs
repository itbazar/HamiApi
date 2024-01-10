namespace Api.Contracts.WebContents;

public record AddWebContentDto(
    string Title,
    string Description = "",
    string Content = "");