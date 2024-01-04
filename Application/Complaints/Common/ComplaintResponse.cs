using Application.ExtensionMethods;
using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Common;

public class ComplaintResponse
{
    public ComplaintResponse(string trackingNumber, string title, ComplaintCategoryResponse complaintCategoryResponse, ComplaintState status, DateTime registeredAt, DateTime lastChanged, string complaining, ComplaintOrganizationResponse complaintOrganizationResponse, List<ComplaintContentResponse> complaintContentResponses, List<ComplaintOperation> complaintOperations)
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
    }

    public string TrackingNumber { get; set; } = null!;
    string Title { get; set; } = null!;
    ComplaintCategoryResponse Category { get; set; } = null!;
    ComplaintState Status { get; set; }
    DateTime RegisteredAt { get; set; }
    DateTime LastChanged { get; set; }
    string Complaining { get; set; } = null!;
    ComplaintOrganizationResponse ComplaintOrganization { get; set; } = null!;
    List<ComplaintContentResponse> Contents { get; set; } = null!;
    List<ComplaintOperation> PossibleOperations { get; set; } = null!;
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
};
