using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByTitleQuery;

internal sealed class GetWebContentByTitleQueryHandler : IRequestHandler<GetWebContentByTitleQuery, WebContent>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetWebContentByTitleQueryHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WebContent> Handle(GetWebContentByTitleQuery request, CancellationToken cancellationToken)
    {
        var result = await _webContentRepository.GetFirstAsync(wc => wc.Title == request.Title);
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}