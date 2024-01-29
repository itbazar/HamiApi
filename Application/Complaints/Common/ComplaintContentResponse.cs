using Domain.Models.ComplaintAggregate;
using SharedKernel.ExtensionMethods;

namespace Application.Complaints.Common;

public class ComplaintContentResponse
{
    public string Text { get; set; } = string.Empty;
    public List<MediaResponse> Media { get; set; } = null!;
    public Actor Sender { get; set; }
    public ComplaintOperation Operation { get; set; }
    public DateTime DateTime { get; set; }
    public EnumValueDescription SenderWithDescription
    {
        get
        {
            return new EnumValueDescription((int)Sender, Sender.GetDescription() ?? "");
        }
    }
    public EnumValueDescription OperationWithDescription
    {
        get
        {
            return new EnumValueDescription((int)Operation, Operation.GetDescription() ?? "");
        }
    }
}
