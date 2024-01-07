using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetAdminSlidersQuery;

internal class GetAdminSlidersQueryHandler : IRequestHandler<GetAdminSlidersQuery, List<Slider>>
{
    private readonly ISliderRepository _sliderRepository;

    public GetAdminSlidersQueryHandler(ISliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }

    public async Task<List<Slider>> Handle(GetAdminSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await _sliderRepository.GetAsync(null, false);
        return sliders.ToList();
    }
}