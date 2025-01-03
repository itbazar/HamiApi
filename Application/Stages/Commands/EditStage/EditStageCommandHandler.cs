
using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using FluentResults;
using MediatR;

namespace Application.Stages.Commands.EditStage;

internal class EditStageCommandHandler : IRequestHandler<EditStageCommand, Result<Stage>>
{
    private readonly IStageRepository _diseaseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditStageCommandHandler(IStageRepository diseaseRepository, IUnitOfWork unitOfWork)
    {
        _diseaseRepository = diseaseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Stage>> Handle(EditStageCommand request, CancellationToken cancellationToken)
    {
        // دریافت Stage از مخزن
        var disease = await _diseaseRepository.GetSingleAsync(d => d.Id == request.Id);

        // بررسی وجود Stage
        if (disease is null)
            return GenericErrors.NotFound;

        // بروزرسانی Stage
        disease.Update(request.Title, request.Description);

        // ذخیره تغییرات
        _diseaseRepository.Update(disease);
        await _unitOfWork.SaveAsync();

        return disease;
    }
}