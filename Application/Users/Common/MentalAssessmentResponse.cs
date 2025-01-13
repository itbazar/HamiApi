using Application.Complaints.Common;
using SharedKernel.ExtensionMethods;

namespace Application.Users.Common;

public class MentalAssessmentResponse
{
    public List<string> Dates { get; set; } = new();
    public List<int> GadScores { get; set; } = new();
    public List<int> MddScores { get; set; } = new();

}
