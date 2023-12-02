using Domain.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Storage;

public interface IStorageService
{
    Task<ICollection<Media>> WriteFileAsync(ICollection<IFormFile> files, AttachmentType attachmentType);
    Task<Media?> WriteFileAsync(IFormFile file, AttachmentType attachmentType);
}
