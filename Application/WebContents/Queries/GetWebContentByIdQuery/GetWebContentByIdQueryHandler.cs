using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByIdQuery;

internal sealed class GetWebContentByIdQueryHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork) : IRequestHandler<GetWebContentByIdQuery, Result<WebContent>>
{
    public async Task<Result<WebContent>> Handle(GetWebContentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await webContentRepository.GetSingleAsync(wc => wc.Id == request.Id);
        if (result is null)
            return GenericErrors.NotFound;
        return result;
    }
}
