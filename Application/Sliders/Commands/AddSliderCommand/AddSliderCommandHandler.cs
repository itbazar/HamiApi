using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using Infrastructure.Storage;

namespace Application.Sliders.Commands.AddSliderCommand;

internal class AddSliderCommandHandler(
    ISliderRepository sliderRepository,
    IStorageService storageService,
    IUnitOfWork unitOfWork) : IRequestHandler<AddSliderCommand, Result<Slider>>
{
    public async Task<Result<Slider>> Handle(AddSliderCommand request, CancellationToken cancellationToken)
    {
        var image = await storageService.WriteFileAsync(request.Image, AttachmentType.Slider);
        if(image is null)
        {
            return GenericErrors.AttachmentFailed;
        }
        var slider = Slider.Create(request.Title, image, request.Url, request.Description);
        sliderRepository.Insert(slider);
        await unitOfWork.SaveAsync();
        return slider;
    }
}