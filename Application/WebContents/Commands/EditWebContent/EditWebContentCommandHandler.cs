using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.EditWebContent;

internal sealed class EditWebContentCommandHandler : IRequestHandler<EditWebContentCommand, WebContent>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditWebContentCommandHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WebContent> Handle(EditWebContentCommand request, CancellationToken cancellationToken)
    {
        var webContent = await _webContentRepository.GetSingleAsync(wc => wc.Id == request.Id);
        if (webContent is null)
            throw new Exception("Not found!");
        webContent.Update(request.Title, request.Description, request.Content);
        _webContentRepository.Update(webContent);
        await _unitOfWork.SaveAsync();
        return webContent;
    }
}