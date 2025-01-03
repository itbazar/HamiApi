using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientGroups.Commands.DeletePatientGroupCommand;

internal class DeletePatientGroupCommandHandler(
    IPatientGroupRepository patientGrouprRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePatientGroupCommand, Result<PatientGroup>>
{
    public async Task<Result<PatientGroup>> Handle(DeletePatientGroupCommand request, CancellationToken cancellationToken)
    {
        var patientGroupr = await patientGrouprRepository.GetSingleAsync(s => s.Id == request.Id);
        if (patientGroupr is null)
            return GenericErrors.NotFound;
        patientGroupr.Delete(request.IsDeleted.Value);
        patientGrouprRepository.Update(patientGroupr);
        await unitOfWork.SaveAsync();
        return patientGroupr;
    }
}