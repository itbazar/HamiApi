using SixLabors.ImageSharp;

namespace Infrastructure.Storage;

public class ImageQualityOptions
{
    public List<Size> ImageQualities { get; set; } = new List<Size>();
}
