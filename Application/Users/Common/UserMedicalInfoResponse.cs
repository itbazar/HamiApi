using SharedKernel.ExtensionMethods;

namespace Application.Users.Common;

public class UserMedicalInfoResponse
{
    public string UserName { get; set; }
    public string GroupName { get; set; }
    public string Organ { get; set; }
    public string DiseaseType { get; set; }
    public string PatientStatus { get; set; }
    public int? Stage { get; set; }
    public string PathologyDiagnosis { get; set; }
    public float? InitialWeight { get; set; }
    public int? SleepDuration { get; set; }
    public string AppetiteLevel { get; set; }
    public int? GadScore { get; set; }
    public int? MddScore { get; set; }

}
