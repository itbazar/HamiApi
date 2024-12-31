
using Application.Common.Interfaces.Persistence;
using Domain.Models.StageAggregate;
using MediatR;

namespace Application.Stages.Commands.AddStage;

internal class AddStageCommandHandler(IStageRepository diseaseRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddStageCommand, Result<Stage>>
{
    public async Task<Result<Stage>> Handle(AddStageCommand request, CancellationToken cancellationToken)
    {
        var disease = Stage.Create(request.Title, request.Description);
        diseaseRepository.Insert(disease);
        await unitOfWork.SaveAsync();
        return disease;
    }
}