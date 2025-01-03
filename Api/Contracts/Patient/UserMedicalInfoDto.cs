using Domain.Models.Hami;

namespace Api.Contracts.Patient
{
    public record UserMedicalInfoDto(
    string UserId="",
    string Organ = "",
    string DiseaseType = "",
    string PatientStatus = "");
}
