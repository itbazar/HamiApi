using Domain.Models.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.Uploads.Commands.CreateUpload;

internal sealed class AddUploadCommandHandler : IRequestHandler<AddUploadCommand, StorageMedia>
{
    private readonly IStorageService _storageService;

    public AddUploadCommandHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<StorageMedia> Handle(AddUploadCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.File, request.AttachmentType);
        if (media == null)
        {
            throw new Exception("Upload failed.");
        }

        return media;
    }
}
