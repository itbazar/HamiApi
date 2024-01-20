using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;

namespace Application.Sliders.Commands.DeleteSliderCommand;

internal class DeleteSliderCommandHandler(
    ISliderRepository sliderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteSliderCommand, Result<Slider>>
{
public async Task<Result<Slider>> Handle(DeleteSliderCommand request, CancellationToken cancellationToken)
    {
        var slider = await sliderRepository.GetSingleAsync(s => s.Id == request.Id);
        if (slider is null)
            return GenericErrors.NotFound;
        slider.Delete(request.IsDeleted);
        sliderRepository.Update(slider);
        await unitOfWork.SaveAsync();
        return slider;
    }
}