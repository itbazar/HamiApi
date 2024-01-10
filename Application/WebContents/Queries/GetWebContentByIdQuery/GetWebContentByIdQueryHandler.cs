using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByIdQuery;

internal sealed class GetWebContentByIdQueryHandler : IRequestHandler<GetWebContentByIdQuery, WebContent>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetWebContentByIdQueryHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WebContent> Handle(GetWebContentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _webContentRepository.GetSingleAsync(wc => wc.Id == request.Id);
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}
