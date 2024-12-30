using Application.Common.Interfaces.Persistence;
using Domain.Models.DiseaseAggregate;
using MediatR;

namespace Application.Diseases.Commands.AddDisease;

internal class AddDiseaseCommandHandler(IDiseaseRepository diseaseRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddDiseaseCommand, Result<Disease>>
{
    public async Task<Result<Disease>> Handle(AddDiseaseCommand request, CancellationToken cancellationToken)
    {
        var disease = Disease.Create(request.Title, request.Description);
        diseaseRepository.Insert(disease);
        await unitOfWork.SaveAsync();
        return disease;
    }
}