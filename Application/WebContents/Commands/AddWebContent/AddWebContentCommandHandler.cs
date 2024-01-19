using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.AddWebContent;

internal sealed class AddWebContentCommandHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddWebContentCommand, Result<WebContent>>
{
    public async Task<Result<WebContent>> Handle(AddWebContentCommand request, CancellationToken cancellationToken)
    {
        var webContent = WebContent.Create(request.Title, request.Description, request.Content);
        webContentRepository.Insert(webContent);
        await unitOfWork.SaveAsync();
        return webContent;
    }
}
