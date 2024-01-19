using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetAdminSlidersQuery;

internal class GetAdminSlidersQueryHandler(ISliderRepository sliderRepository) : IRequestHandler<GetAdminSlidersQuery, Result<List<Slider>>>
{
    public async Task<Result<List<Slider>>> Handle(GetAdminSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await sliderRepository.GetAsync(null, false);
        return sliders.ToList();
    }
}