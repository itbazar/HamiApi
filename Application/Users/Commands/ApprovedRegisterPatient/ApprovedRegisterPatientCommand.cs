using Application.Users.Common;


namespace Application.Users.Commands.ApprovedRegisterPatient;

public record ApprovedRegisterPatientCommand(
    string UserId,
    Guid? PatientGroupId,
    bool IsApproved,
    string? RejectionReason) : IRequest<Result<AddPatientResult>>;
