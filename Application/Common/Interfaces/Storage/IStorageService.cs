using Domain.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Storage;

public interface IStorageService
{
    Task<ICollection<StorageMedia>> WriteFileAsync(ICollection<IFormFile> files, AttachmentType attachmentType);
    Task<StorageMedia?> WriteFileAsync(IFormFile file, AttachmentType attachmentType);
}
