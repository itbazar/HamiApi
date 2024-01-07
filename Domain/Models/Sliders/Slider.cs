using Domain.Models.Common;
using Domain.Primitives;

namespace Domain.Models.Sliders;

public class Slider : Entity
{
	private Slider(Guid id) : base(id) { }

    public static Slider Create(
        string title,
        StorageMedia image,
        string url = "",
        string description = "")
	{
		var slider = new Slider(Guid.Empty);
        slider.Title = title;
        slider.Description = description;
        slider.Image = image;
        slider.Url = url;
		return slider;
	}

    public void Delete(bool? isDeleted)
    {
        IsDeleted = isDeleted ?? !IsDeleted;
    }

    public void Update(
        string? title,
        StorageMedia? image,
        string? url = "",
        string? description = "")
    {
        Title = title ?? Title;
        Image = image ?? Image;
        Url = url ?? Url;
        Description = description ?? Description;
    }

    public StorageMedia Image { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
