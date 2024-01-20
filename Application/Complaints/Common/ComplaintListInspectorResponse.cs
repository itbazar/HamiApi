using Domain.Models.ComplaintAggregate;
using SharedKernel.ExtensionMethods;

namespace Application.Complaints.Common;

public class ComplaintListInspectorResponse
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ComplaintCategoryResponse Category { get; set; } = null!;
    public ComplaintState Status { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastChanged { get; set; }
    public Actor LastActor { get; set; }
    public string Complaining { get; set; } = null!;
    public ComplaintOrganizationResponse ComplaintOrganization { get; set; } = null!;
    public byte[] CipherKeyInspector { get; set; } = null!;

    public EnumValueDescription LastActorWithDescription
    {
        get
        {
            return new EnumValueDescription((int)LastActor, LastActor.GetDescription() ?? "");
        }
    }

    public EnumValueDescription StatusWithDescription
    {
        get
        {
            return new EnumValueDescription((int)Status, Status.GetDescription() ?? "");
        }
    }
}
