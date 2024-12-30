using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;
using FluentResults;
using MediatR;

namespace Application.Diseases.Commands.EditDisease;

internal class EditDiseaseCommandHandler : IRequestHandler<EditDiseaseCommand, Result<Disease>>
{
    private readonly IDiseaseRepository _diseaseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditDiseaseCommandHandler(IDiseaseRepository diseaseRepository, IUnitOfWork unitOfWork)
    {
        _diseaseRepository = diseaseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Disease>> Handle(EditDiseaseCommand request, CancellationToken cancellationToken)
    {
        // دریافت Disease از مخزن
        var disease = await _diseaseRepository.GetSingleAsync(d => d.Id == request.Id);

        // بررسی وجود Disease
        if (disease is null)
            return GenericErrors.NotFound;

        // بروزرسانی Disease
        disease.Update(request.Title, request.Description);

        // ذخیره تغییرات
        _diseaseRepository.Update(disease);
        await _unitOfWork.SaveAsync();

        return disease;
    }
}
