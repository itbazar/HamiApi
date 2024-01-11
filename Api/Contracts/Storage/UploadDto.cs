using Infrastructure.Storage;

namespace Api.Contracts.Storage;

public record UploadDto(IFormFile File, AttachmentType AttachmentType);


