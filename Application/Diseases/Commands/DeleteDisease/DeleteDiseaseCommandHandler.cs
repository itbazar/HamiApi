using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;
using MediatR;
using FluentResults;

namespace Application.Diseases.Commands.DeleteDisease;

internal class DeleteDiseaseCommandHandler(IDiseaseRepository diseaseRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDiseaseCommand, Result<Disease>>
{
    public async Task<Result<Disease>> Handle(DeleteDiseaseCommand request, CancellationToken cancellationToken)
    {
        // دریافت Disease از دیتابیس
        var disease = await diseaseRepository.GetSingleAsync(d => d.Id == request.Id);
        if (disease is null)
            throw new Exception("Not found!");

        // حذف منطقی Disease
        disease.Delete(request.IsDeleted);

        // بروزرسانی وضعیت در دیتابیس
        diseaseRepository.Update(disease);
        await unitOfWork.SaveAsync();

        return Result.Ok(disease);
    }
}
