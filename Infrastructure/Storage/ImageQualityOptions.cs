using SixLabors.ImageSharp;

namespace Infrastructure.Storage;

public class ImageQualityOptions
{
    public const string Name = "ImageQualityOptions";

    public List<Size> ImageQualities { get; set; } = new List<Size>();
}
