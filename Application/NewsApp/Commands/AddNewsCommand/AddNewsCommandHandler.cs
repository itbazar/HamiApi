using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using Infrastructure.Storage;

namespace Application.NewsApp.Commands.AddNewsCommand;

internal class AddNewsCommandHandler(
    INewsRepository newsRepository,
    IStorageService storageService,
    IUnitOfWork unitOfWork) : IRequestHandler<AddNewsCommand, Result<News>>
{
    public async Task<Result<News>> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var image = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (image is null)
        {
            return GenericErrors.AttachmentFailed;
        }
        var news = News.Create(request.Title, image, request.Url, request.Description);
        newsRepository.Insert(news);
        await unitOfWork.SaveAsync();
        return news;
    }
}