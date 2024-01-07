using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetSlidersQuery;

internal class GetSlidersQueryHandler : IRequestHandler<GetSlidersQuery, List<Slider>>
{
    private readonly ISliderRepository _sliderRepository;

    public GetSlidersQueryHandler(ISliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }

    public async Task<List<Slider>> Handle(GetSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await _sliderRepository.GetAsync(s => s.IsDeleted == false, false);
        return sliders.ToList();
    }
}