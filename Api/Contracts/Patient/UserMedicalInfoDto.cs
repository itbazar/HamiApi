using Domain.Models.Hami;

namespace Api.Contracts.Patient
{
    public record UserMedicalInfoDto(
    string UserId = "",
    string Organ = "",
    string DiseaseType = "",
    string PatientStatus = "",
    int Stage=0,
    string PathologyDiagnosis = "",
     int InitialWeight = 0,
     int? SleepDuration=0,
      string AppetiteLevel = "");
}
