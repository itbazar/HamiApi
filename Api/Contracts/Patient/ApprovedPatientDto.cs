using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Api.Contracts.Patient
{
    public class ApprovedPatientDto
    {
        public string UserId { get; set; } = string.Empty;
        public Guid PatientGroupId { get; set; } 
        public bool IsApproved { get; set; } = false;
        public string RejectionReason { get; set; } = string.Empty; 
    }
}
