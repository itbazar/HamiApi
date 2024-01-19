using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.News;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.UpdateNewsCommand;

internal class UpdateNewsCommandHandler(
    INewsRepository newsRepository,
    IStorageService storageService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateNewsCommand, Result<News>>
{
    public async Task<Result<News>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        StorageMedia? image = null;
        if (request.Image is not null)
        {
            image = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (image is null)
            {
                return GenericErrors.AttachmentFailed;
            }
        }

        var news = await newsRepository.GetSingleAsync(s => s.Id == request.Id);
        if (news is null)
            throw new Exception("Not found.");
        news.Update(request.Title, image, request.Url, request.Description);
        newsRepository.Update(news);
        await unitOfWork.SaveAsync();
        return news;
    }
}