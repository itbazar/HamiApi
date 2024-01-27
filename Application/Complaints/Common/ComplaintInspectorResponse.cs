using Domain.Models.ComplaintAggregate;
using Mapster;
using SharedKernel.ExtensionMethods;

namespace Application.Complaints.Common;

public class ComplaintInspectorResponse
{
    public static ComplaintInspectorResponse FromComplaint(Complaint complaint)
    {
        return new ComplaintInspectorResponse(complaint.TrackingNumber,
            complaint.Title,
            complaint.Category.Adapt<ComplaintCategoryResponse>(),
            complaint.Status,
            complaint.RegisteredAt,
            complaint.LastChanged,
            complaint.Complaining,
            complaint.ComplaintOrganization.Adapt<ComplaintOrganizationResponse>(),
            complaint.Contents.Adapt<List<ComplaintContentResponse>>(),
            complaint.GetPossibleOperations(Actor.Inspector),
            complaint.User.Adapt<UserResponse>());
    }
    private ComplaintInspectorResponse(
        string trackingNumber,
        string title,
        ComplaintCategoryResponse complaintCategoryResponse,
        ComplaintState status,
        DateTime registeredAt,
        DateTime lastChanged,
        string complaining,
        ComplaintOrganizationResponse complaintOrganizationResponse,
        List<ComplaintContentResponse> complaintContentResponses,
        List<ComplaintOperation> complaintOperations,
        UserResponse user)
    {
        TrackingNumber = trackingNumber;
        Title = title;
        Category = complaintCategoryResponse;
        Status = status;
        RegisteredAt = registeredAt;
        LastChanged = lastChanged;
        Complaining = complaining;
        ComplaintOrganization = complaintOrganizationResponse;
        Contents = complaintContentResponses;
        PossibleOperations = complaintOperations;
        User = user;
    }

    public string TrackingNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ComplaintCategoryResponse Category { get; set; } = null!;
    public ComplaintState Status { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastChanged { get; set; }
    public string Complaining { get; set; } = null!;
    public ComplaintOrganizationResponse ComplaintOrganization { get; set; } = null!;
    public List<ComplaintContentResponse> Contents { get; set; } = null!;
    public List<ComplaintOperation> PossibleOperations { get; set; } = null!;
    public EnumValueDescription StatusWithDescription
    {
        get
        {
            return new EnumValueDescription((int)Status, Status.GetDescription() ?? "");
        }
    }
    public List<EnumValueDescription> PossibleOperationsWithDescription
    {
        get
        {
            var result = new List<EnumValueDescription>();
            PossibleOperations.ForEach(o => { result.Add(new EnumValueDescription((int)o, o.GetDescription() ?? "")); });
            return result;
        }
    }
    public UserResponse User { get; set; } = null!;
};
