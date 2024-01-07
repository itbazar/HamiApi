using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetSliderByIdQuery;

internal class GetSliderByIdQueryHandler : IRequestHandler<GetSliderByIdQuery, Slider>
{
    private readonly ISliderRepository _sliderRepository;

    public GetSliderByIdQueryHandler(ISliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }

    public async Task<Slider> Handle(GetSliderByIdQuery request, CancellationToken cancellationToken)
    {
        var slider = await _sliderRepository.GetSingleAsync(s => s.Id == request.Id, false);
        if (slider is null)
            throw new Exception("Not found.");
        return slider;
    }
}