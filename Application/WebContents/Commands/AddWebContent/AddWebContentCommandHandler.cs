using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.AddWebContent;

internal sealed class AddWebContentCommandHandler : IRequestHandler<AddWebContentCommand, WebContent>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddWebContentCommandHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WebContent> Handle(AddWebContentCommand request, CancellationToken cancellationToken)
    {
        var webContent = WebContent.Create(request.Title, request.Description, request.Content);
        _webContentRepository.Insert(webContent);
        await _unitOfWork.SaveAsync();
        return webContent;
    }
}
