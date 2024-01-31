using SixLabors.ImageSharp;

namespace Infrastructure.Options;

public class StorageOptions
{
    public static string Name = "Storage";
    public List<Size> ImageQualities { get; set; } = new List<Size>();
    public string AllowedExtensions { get; set; } = string.Empty;
    public int MaxFileCount { get; set; }
    public long MaxFileSize { get; set; }
    public int MaxTextLength { get; set; }
}

