
using Application.Common.Interfaces.Persistence;
using Domain.Models.StageAggregate;
using MediatR;
using FluentResults;

namespace Application.Stages.Commands.DeleteStage;

internal class DeleteStageCommandHandler(IStageRepository diseaseRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteStageCommand, Result<Stage>>
{
    public async Task<Result<Stage>> Handle(DeleteStageCommand request, CancellationToken cancellationToken)
    {
        // دریافت Stage از دیتابیس
        var disease = await diseaseRepository.GetSingleAsync(d => d.Id == request.Id);
        if (disease is null)
            throw new Exception("Not found!");

        // حذف منطقی Stage
        disease.Delete(request.IsDeleted);

        // بروزرسانی وضعیت در دیتابیس
        diseaseRepository.Update(disease);
        await unitOfWork.SaveAsync();

        return Result.Ok(disease);
    }
}