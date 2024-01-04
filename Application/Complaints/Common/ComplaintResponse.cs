using Application.ExtensionMethods;
using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Common;

public record ComplaintResponse(
    string TrackingNumber,
    string Title,
    ComplaintCategoryResponse Category,
    ComplaintState Status,
    DateTime RegisteredAt,
    DateTime LastChanged,
    string Complaining,
    ComplaintOrganization ComplaintOrganization,
    List<ComplaintContentResponse> Contents,
    List<ComplaintOperation> PossibleOperations)
{
    //public EnumValueDescription StatusWithDescription
    //{ 
    //    get
    //    {
    //        return new EnumValueDescription((int)Status, Status.GetDescription() ?? "");
    //    } 
    //}
    //public List<EnumValueDescription> PossibleOperationsWithDescription
    //{ 
    //    get 
    //    { 
    //        var result = new List<EnumValueDescription>();
    //        PossibleOperations.ForEach(o => { result.Add(new EnumValueDescription((int)o, o.GetDescription() ?? "")); });
    //        return result;
    //    } 
    //}
};
