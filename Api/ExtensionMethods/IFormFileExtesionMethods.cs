using Application.Complaints.Commands.Common;
using Domain.Models.Common;
using Infrastructure.Options;

namespace Api.ExtensionMethods;

public static class IFormFileExtesionMethods
{
    public static MediaRequest GetMedia(
        this IFormFile file)
    {
        var tmp = new MemoryStream();
        file.CopyTo(tmp);
        var mediaRequest = new MediaRequest(file.FileName, file.ContentType, MediaType.Image, tmp.ToArray());
        return mediaRequest;
    }

    public static List<MediaRequest> GetMedia(
        this List<IFormFile>? files,
        long maxFileSize,
        int maxFileCount,
        List<string> allowedExtensions)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if (files is null)
            return data;
        
        if (files.Count > maxFileCount)
        {
            throw new Exception("Max file count limit exceeded.");
        }
        foreach (var file in files)
        {
            if (file.Length > maxFileSize)
                throw new Exception("Max file size limit exceeded.");
            if (!allowedExtensions.Contains(file.FileName.Split('.').Last().ToUpper()))
            {
                throw new Exception("Unacceptable file type.");
            }
            data.Add(file.GetMedia());
        }
        return data;
    }

    public static List<MediaRequest> GetMedia(
        this List<IFormFile>? files,
        StorageOptions storageOptions)
    {
        var allowedExtensions = storageOptions.AllowedExtensions.Split(',')
            .ToList()
            .Select(x => x.Trim())
            .ToList();
        List<MediaRequest> data = new List<MediaRequest>();
        if (files is null)
            return data;

        if (files.Count > storageOptions.MaxFileCount)
        {
            throw new Exception("Max file count limit exceeded.");
        }
        foreach (var file in files)
        {
            if (file.Length > storageOptions.MaxFileSize)
                throw new Exception("Max file size limit exceeded.");
            if (!allowedExtensions.Contains(file.FileName.Split('.').Last().ToUpper()))
            {
                throw new Exception("Unacceptable file type.");
            }
            data.Add(file.GetMedia());
        }
        return data;
    }
}