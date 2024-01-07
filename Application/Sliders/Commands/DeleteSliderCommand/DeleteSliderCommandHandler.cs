using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Commands.UpdateSliderCommand;

internal class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommand, Slider>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSliderCommandHandler(
        ISliderRepository sliderRepository,
        IUnitOfWork unitOfWork)
    {
        _sliderRepository = sliderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Slider> Handle(DeleteSliderCommand request, CancellationToken cancellationToken)
    {
        var slider = await _sliderRepository.GetSingleAsync(s => s.Id == request.Id);
        if (slider is null)
            throw new Exception("Not found.");
        slider.Delete(request.IsDeleted);
        _sliderRepository.Update(slider);
        await _unitOfWork.SaveAsync();
        return slider;
    }
}