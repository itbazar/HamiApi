namespace Domain.Models.Common;

public class StorageMedia
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Url2 { get; set; } = string.Empty;
    public string Url3 { get; set; } = string.Empty;
    public string Url4 { get; set; } = string.Empty;
    public string AlternateText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
}

