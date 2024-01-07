using Domain.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly string _destinationPath;
    private readonly List<Size> _imageQualities;
    private const long maxAllowdFileSize = 10 * 1024 * 1024;

    public StorageService(string destinationPath, List<Size>? imageQualities)
    {
        _destinationPath = destinationPath;
        _imageQualities = imageQualities ?? new List<Size>() { new Size(100, 100), new Size(200, 200), new Size(300, 300) };
    }

    public async Task<ICollection<StorageMedia>> WriteFileAsync(ICollection<IFormFile> files, AttachmentType attachmentType)
    {
        var result = new List<StorageMedia>();
        foreach (var file in files)
        {
            var r = await WriteFileAsync(file, attachmentType);
            if (r != null)
            {
                result.Add(r);
            }
        }

        return result;
    }

    public async Task<StorageMedia?> WriteFileAsync(IFormFile file, AttachmentType attachmentType)
    {
        //if (!isAcceptedExtension(file.FileName))
        //    return null;
        if (file == null)
            return null;

        if (file.Length > maxAllowdFileSize)
        {
            return null;
        }

        if (!isAcceptedExtension(file.FileName))
        {
            return null;
        }

        string fileName;
        string path;
        string sub;
        switch (attachmentType)
        {
            case AttachmentType.Avatar:
                sub = "Avatar";
                break;
            case AttachmentType.Slider:
                sub = "Slider";
                break;
            case AttachmentType.News:
                sub = "News";
                break;
            default:
                sub = "Misc";
                break;
        }
        string relativePath;
        StorageMedia result;
        try
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            //var extension = ".jpg";
            fileName = DateTime.Now.Ticks.ToString(); //Create a new Name for the file due to security reasons.
            relativePath = Path.Combine("Attachments", sub);
            var pathBuilt = Path.Combine(_destinationPath, relativePath);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            path = Path.Combine(pathBuilt,
               fileName);

            if (!isImageFile(extension))
            {
                path = path + extension;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    result = new StorageMedia()
                    {
                        AlternateText = "",
                        MediaType = GetMediaType(extension),
                        Title = "",
                        Url = Path.Combine(relativePath, fileName + extension)
                    };
                }
            }
            else
            {
                Image image = Image.Load(file.OpenReadStream());
                result = await writeImage(image, _destinationPath, relativePath, fileName, _imageQualities);
            }

        }
        catch(Exception e)
        {
            throw e.InnerException!;
        }

        return result;
    }

    private static MediaType GetMediaType(string extension)
    {
        var videoExtensions = new List<string> { ".mkv", ".mp4", ".mov", ".3gp", ".ogg" };
        var docExtensions = new List<string> { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx" };

        if (videoExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Video;
        }

        if (docExtensions.Contains(extension.ToLower()))
        {
            return MediaType.Doc;
        }

        return MediaType.Other;
    }

    private static bool isAcceptedExtension(string fileName)
    {
        var validExtensions = new List<string>()
            {
                "jpg", "jpeg", "jpe", "jif", "jfif", "jfi", "png", "gif", "tiff", "tif", "svg", "svgz",
                "pdf", "doc", "docx", "ppt", "pptx", "xls", "xlsx",
                "mkv", "mp4", "mov", "3gp", "ogg"
            };
        var extension = fileName.Split('.')[fileName.Split('.').Length - 1];

        return validExtensions.Contains(extension.ToLower());
    }

    private async Task<StorageMedia> writeImage(Image image, string destinationPath, string relativePath, string fileName, List<Size> imageQualities)
    {
        var resolutions = new List<Tuple<string, System.Drawing.Size>>();
        for (int i = 0; i < imageQualities.Count; i++)
        {
            resolutions.Add(new Tuple<string, System.Drawing.Size>((i + 1).ToString(), new System.Drawing.Size(imageQualities[i].Width, imageQualities[i].Height)));
        }

        StorageMedia result;
        Image thumb;
        JpegEncoder jpegEncoder = new JpegEncoder();
        using MemoryStream mStream = new MemoryStream();
        image.Save(mStream, jpegEncoder);
        string fullPath = Path.Combine(destinationPath, relativePath, fileName);
        string pathBuilt = Path.Combine(destinationPath, relativePath);
        if (!Directory.Exists(pathBuilt))
        {
            Directory.CreateDirectory(pathBuilt);
        }
        await writeToDisk(mStream, fullPath + ".jpg");

        foreach (var item in resolutions)
        {
            var xRatio = (double)item.Item2.Width / image.Width;
            var yRatio = (double)item.Item2.Height / image.Height;
            var ratio = Math.Min(xRatio, yRatio);
            var width = (int)(image.Width * ratio);
            var height = (int)(image.Height * ratio);

            mStream.Seek(0, SeekOrigin.Begin);
            mStream.SetLength(0);
            thumb = image.Clone(x => x.Resize(width, height));
            thumb.Save(mStream, jpegEncoder);
            await writeToDisk(mStream, fullPath + item.Item1 + ".jpg");
        }

        string fullRelativePath = Path.Combine(relativePath, fileName);
        result = new StorageMedia()
        {
            AlternateText = "",
            MediaType = MediaType.Image,
            Title = "",
            Url = fullRelativePath + ".jpg",
            Url2 = fullRelativePath + "1" + ".jpg",
            Url3 = fullRelativePath + "2" + ".jpg",
            Url4 = fullRelativePath + "3" + ".jpg",
        };

        return result;
    }

    private async Task writeToDisk(MemoryStream mStream, string path)
    {
        using var outStream = new FileStream(path, FileMode.Create);
        mStream.Seek(0, SeekOrigin.Begin);
        await mStream.CopyToAsync(outStream);
    }

    private bool isImageFile(string extension)
    {
        var imageExtensions = new List<string>()
            {
                ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".gif", ".tiff", ".tif", ".svg", ".svgz"
            };

        if (imageExtensions.Contains(extension.ToLower()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
