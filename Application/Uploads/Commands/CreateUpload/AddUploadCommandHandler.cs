using Application.Common.Errors;
using Domain.Models.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.Uploads.Commands.CreateUpload;

internal sealed class AddUploadCommandHandler(IStorageService storageService) : IRequestHandler<AddUploadCommand, Result<StorageMedia>>
{
    public async Task<Result<StorageMedia>> Handle(AddUploadCommand request, CancellationToken cancellationToken)
    {
        var media = await storageService.WriteFileAsync(request.File, request.AttachmentType);
        if (media == null)
        {
            return GenericErrors.AttachmentFailed;
        }

        return media;
    }
}
