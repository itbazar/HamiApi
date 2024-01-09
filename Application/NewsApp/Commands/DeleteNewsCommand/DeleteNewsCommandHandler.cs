using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Commands.DeleteNewsCommand;

internal class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, News>
{
    private readonly INewsRepository _newsrRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNewsCommandHandler(
        INewsRepository newsrRepository,
        IUnitOfWork unitOfWork)
    {
        _newsrRepository = newsrRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<News> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var newsr = await _newsrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (newsr is null)
            throw new Exception("Not found.");
        newsr.Delete(request.IsDeleted);
        _newsrRepository.Update(newsr);
        await _unitOfWork.SaveAsync();
        return newsr;
    }
}