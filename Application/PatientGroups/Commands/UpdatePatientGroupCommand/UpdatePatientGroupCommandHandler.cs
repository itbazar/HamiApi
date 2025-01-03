using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.PatientGroups.Commands.UpdatePatientGroupCommand;

internal class UpdatePatientGroupCommandHandler(
    IPatientGroupRepository patientGroupRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdatePatientGroupCommand, Result<PatientGroup>>
{
    public async Task<Result<PatientGroup>> Handle(UpdatePatientGroupCommand request, CancellationToken cancellationToken)
    {
        var patientGroup = await patientGroupRepository.GetSingleAsync(s => s.Id == request.Id);
        if (patientGroup is null)
            throw new Exception("Not found.");
        patientGroup.Update(
            request.Organ,
            request.DiseaseType,
            request.Stage,
            request.Description,
            request.MentorId);

        patientGroupRepository.Update(patientGroup);
        await unitOfWork.SaveAsync();
        return patientGroup;
    }
}