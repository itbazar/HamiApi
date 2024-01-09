using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.News;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.UpdateNewsCommand;

internal class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, News>
{
    private readonly INewsRepository _newsRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNewsCommandHandler(
        INewsRepository newsRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<News> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        StorageMedia? image = null;
        if (request.Image is not null)
        {
            image = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (image is null)
            {
                throw new Exception("Attachment failed.");
            }
        }

        var news = await _newsRepository.GetSingleAsync(s => s.Id == request.Id);
        if (news is null)
            throw new Exception("Not found.");
        news.Update(request.Title, image, request.Url, request.Description);
        _newsRepository.Update(news);
        await _unitOfWork.SaveAsync();
        return news;
    }
}