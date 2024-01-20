using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;

namespace Application.Sliders.Queries.GetSliderByIdQuery;

internal class GetSliderByIdQueryHandler(ISliderRepository sliderRepository) : IRequestHandler<GetSliderByIdQuery, Result<Slider>>
{
    public async Task<Result<Slider>> Handle(GetSliderByIdQuery request, CancellationToken cancellationToken)
    {
        var slider = await sliderRepository.GetSingleAsync(s => s.Id == request.Id, false);
        if (slider is null)
            return GenericErrors.NotFound;
        return slider;
    }
}