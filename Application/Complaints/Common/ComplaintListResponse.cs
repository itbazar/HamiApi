using Application.ExtensionMethods;
using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Common;

public record ComplaintListResponse(
    Guid Id,
    string TrackingNumber,
    string Title,
    ComplaintCategoryResponse Category,
    ComplaintState Status,
    DateTime RegisteredAt,
    DateTime LastChanged,
    Actor LastActor,
    byte[] CipherKeyInspector)
{
    public EnumValueDescription StatusWithDescription
    {
        get
        {
            return new EnumValueDescription((int)Status, Status.GetDescription() ?? "");
        }
    }
    public EnumValueDescription LastActorWithDescription
    {
        get
        {
            return new EnumValueDescription((int)LastActor, LastActor.GetDescription() ?? "");
        }
    }
}
