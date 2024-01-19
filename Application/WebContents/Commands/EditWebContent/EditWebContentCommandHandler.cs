using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.EditWebContent;

internal sealed class EditWebContentCommandHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork) : IRequestHandler<EditWebContentCommand, Result<WebContent>>
{
    public async Task<Result<WebContent>> Handle(EditWebContentCommand request, CancellationToken cancellationToken)
    {
        var webContent = await webContentRepository.GetSingleAsync(wc => wc.Id == request.Id);
        if (webContent is null)
            return GenericErrors.NotFound;
        webContent.Update(request.Title, request.Description, request.Content);
        webContentRepository.Update(webContent);
        await unitOfWork.SaveAsync();
        return webContent;
    }
}