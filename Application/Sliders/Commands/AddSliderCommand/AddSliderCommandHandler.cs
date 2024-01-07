using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Sliders.Commands.AddSliderCommand;

internal class AddSliderCommandHandler : IRequestHandler<AddSliderCommand, Slider>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddSliderCommandHandler(
        ISliderRepository sliderRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _sliderRepository = sliderRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Slider> Handle(AddSliderCommand request, CancellationToken cancellationToken)
    {
        var image = await _storageService.WriteFileAsync(request.Image, AttachmentType.Slider);
        if(image is null)
        {
            throw new Exception("Attachment failed.");
        }
        var slider = Slider.Create(request.Title, image, request.Url, request.Description);
        _sliderRepository.Insert(slider);
        await _unitOfWork.SaveAsync();
        return slider;
    }
}