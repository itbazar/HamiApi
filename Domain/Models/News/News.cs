using Domain.Models.Common;
using Domain.Primitives;

namespace Domain.Models.News;

public class News : Entity
{
    private News(Guid id) : base(id) { }

    public static News Create(
        string title,
        StorageMedia image,
        string url = "",
        string description = "",
        string content = "")
    {
        var news = new News(Guid.Empty);
        news.Title = title;
        news.Description = description;
        news.Image = image;
        news.Url = url;
        news.Content = content;
        return news;
    }

    public void Delete(bool? isDeleted)
    {
        IsDeleted = isDeleted ?? !IsDeleted;
    }

    public void Update(
        string? title,
        StorageMedia? image,
        string? url = "",
        string? description = "",
        string? content = "")
    {
        Title = title ?? Title;
        Image = image ?? Image;
        Url = url ?? Url;
        Description = description ?? Description;
        Content = content ?? Content;
    }

    public StorageMedia Image { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
