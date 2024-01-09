using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.AddNewsCommand;

internal class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, News>
{
    private readonly INewsRepository _newsRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddNewsCommandHandler(
        INewsRepository newsRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<News> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var image = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (image is null)
        {
            throw new Exception("Attachment failed.");
        }
        var news = News.Create(request.Title, image, request.Url, request.Description);
        _newsRepository.Insert(news);
        await _unitOfWork.SaveAsync();
        return news;
    }
}