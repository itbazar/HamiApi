using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetSlidersQuery;

internal class GetSlidersQueryHandler(ISliderRepository sliderRepository) : IRequestHandler<GetSlidersQuery, Result<List<Slider>>>
{
    public async Task<Result<List<Slider>>> Handle(GetSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await sliderRepository.GetAsync(s => s.IsDeleted == false, false);
        return sliders.ToList();
    }
}