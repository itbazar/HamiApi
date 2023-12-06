namespace Domain.Models.ComplaintAggregate;

public class Password
{
    public byte[] Hash { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
}
