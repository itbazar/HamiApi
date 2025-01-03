using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Api.Contracts.Patient
{
    public class RegisterPatientDto
    {
        // اطلاعات مربوط به ApplicationUser
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public EducationLevel? Education { get; set; }
        public string? City { get; set; }

        // اطلاعات مربوط به UserMedicalInfo
        public Organ Organ { get; set; }
        public DiseaseType DiseaseType { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public int? Stage { get; set; }
        public string? PathologyDiagnosis { get; set; }
        public float? InitialWeight { get; set; }
        public int? SleepDuration { get; set; }
        public AppetiteLevel AppetiteLevel { get; set; }
        public int GADScore { get; set; }
        public int MDDScore { get; set; }
    }
}
