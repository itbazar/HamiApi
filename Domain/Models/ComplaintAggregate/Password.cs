namespace Domain.Models.ComplaintAggregate;

public class Password
{
    public string Hash { get; set; } = null!;
    public string Salt { get; set; } = null!;
}
