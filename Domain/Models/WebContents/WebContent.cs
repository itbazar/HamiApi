using Domain.Primitives;

namespace Domain.Models.WebContents;

public class WebContent : Entity
{
    private WebContent(Guid id) : base(id) { }

    public static WebContent Create(string title, string description, string content)
    {
        var webContent = new WebContent(Guid.Empty);
        webContent.Title = title;
        webContent.Description = description;
        webContent.Content = content;
        return webContent;
    }

    public void Update(string? title, string? description, string? content)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        Content = content ?? Content;
    }

    public string Title { get; set; } = string.Empty;
    public string Description {  get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
