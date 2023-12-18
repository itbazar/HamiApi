using Application.Complaints.Commands.Common;
using Domain.Models.Common;

namespace Api.ExtensionMethods;

public static class IFormFileExtesionMethods
{
    public static MediaRequest GetMedia(this IFormFile file)
    {
        var tmp = new MemoryStream();
        file.CopyTo(tmp);
        var mediaRequest = new MediaRequest(file.FileName, MediaType.Image, tmp.ToArray());
        return mediaRequest;
    }
}