using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientGroups.Commands.AddPatientGroupCommand;

internal class AddPatientGroupCommandHandler(
    IPatientGroupRepository patientGroupRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddPatientGroupCommand, Result<PatientGroup>>
{
    public async Task<Result<PatientGroup>> Handle(AddPatientGroupCommand request, CancellationToken cancellationToken)
    {
        var patientGroup = PatientGroup.Create(
            request.Organ,
            request.DiseaseType,
            request.Stage,
            request.Description,
            request.MentorId
        ); 
        patientGroupRepository.Insert(patientGroup);
        await unitOfWork.SaveAsync();
        return patientGroup;
    }
}