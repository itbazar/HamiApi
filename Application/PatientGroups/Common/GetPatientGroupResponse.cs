using Domain.Models.Hami;
using SharedKernel.ExtensionMethods;

namespace Application.PatientGroups.Common;

public class GetPatientGroupResponse
{
    public Guid Id { get; set; }
    public string Organ { get; set; }
    public string DiseaseType { get; set; }
    public int? Stage { get; set; }
    public string Description { get; set; }
    public string MentorName { get; set; }

}
