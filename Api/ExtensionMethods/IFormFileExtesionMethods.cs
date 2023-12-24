using Application.Complaints.Commands.Common;
using Domain.Models.Common;

namespace Api.ExtensionMethods;

public static class IFormFileExtesionMethods
{
    public static MediaRequest GetMedia(this IFormFile file)
    {
        var tmp = new MemoryStream();
        file.CopyTo(tmp);
        var mediaRequest = new MediaRequest(file.FileName, file.ContentType, MediaType.Image, tmp.ToArray());
        return mediaRequest;
    }

    public static List<MediaRequest> GetMedia(this List<IFormFile>? files)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if (files is null)
            return data;

        foreach (var file in files)
        {
            data.Add(file.GetMedia());
        }
        return data;
    }
}