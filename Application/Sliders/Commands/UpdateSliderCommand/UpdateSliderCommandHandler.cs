using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Sliders;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Sliders.Commands.UpdateSliderCommand;

internal class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommand, Slider>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSliderCommandHandler(
        ISliderRepository sliderRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _sliderRepository = sliderRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Slider> Handle(UpdateSliderCommand request, CancellationToken cancellationToken)
    {
        StorageMedia? image = null;
        if(request.Image is not null)
        {
            image = await _storageService.WriteFileAsync(request.Image, AttachmentType.Slider);
            if (image is null)
            {
                throw new Exception("Attachment failed.");
            }
        }

        var slider = await _sliderRepository.GetSingleAsync(s => s.Id == request.Id);
        if (slider is null)
            throw new Exception("Not found.");
        slider.Update(request.Title, image, request.Url, request.Description);
        _sliderRepository.Update(slider);
        await _unitOfWork.SaveAsync();
        return slider;
    }
}