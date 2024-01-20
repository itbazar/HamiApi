using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Sliders;
using Infrastructure.Storage;

namespace Application.Sliders.Commands.UpdateSliderCommand;

internal class UpdateSliderCommandHandler(
    ISliderRepository sliderRepository,
    IStorageService storageService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSliderCommand, Result<Slider>>
{
public async Task<Result<Slider>> Handle(UpdateSliderCommand request, CancellationToken cancellationToken)
    {
        StorageMedia? image = null;
        if(request.Image is not null)
        {
            image = await storageService.WriteFileAsync(request.Image, AttachmentType.Slider);
            if (image is null)
            {
                return GenericErrors.AttachmentFailed;
            }
        }

        var slider = await sliderRepository.GetSingleAsync(s => s.Id == request.Id);
        if (slider is null)
            return GenericErrors.NotFound;
        slider.Update(request.Title, image, request.Url, request.Description);
        sliderRepository.Update(slider);
        await unitOfWork.SaveAsync();
        return slider;
    }
}