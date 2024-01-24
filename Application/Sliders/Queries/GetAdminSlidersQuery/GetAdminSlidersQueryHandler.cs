using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;

namespace Application.Sliders.Queries.GetAdminSlidersQuery;

internal class GetAdminSlidersQueryHandler(ISliderRepository sliderRepository) 
    : IRequestHandler<GetAdminSlidersQuery, Result<PagedList<Slider>>>
{
    public async Task<Result<PagedList<Slider>>> Handle(GetAdminSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await sliderRepository.GetPagedAsync(
            request.PagingInfo,
            null,
            false);
        return sliders;
    }
}