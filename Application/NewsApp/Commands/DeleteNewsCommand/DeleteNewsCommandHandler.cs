using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Application.NewsApp.Commands.DeleteNewsCommand;

internal class DeleteNewsCommandHandler(
    INewsRepository newsrRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteNewsCommand, Result<News>>
{
    public async Task<Result<News>> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var newsr = await newsrRepository.GetSingleAsync(s => s.Id == request.Id);
        if (newsr is null)
            return GenericErrors.NotFound;
        newsr.Delete(request.IsDeleted);
        newsrRepository.Update(newsr);
        await unitOfWork.SaveAsync();
        return newsr;
    }
}