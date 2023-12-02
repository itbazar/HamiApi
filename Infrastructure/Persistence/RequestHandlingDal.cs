namespace Infrastructure.Persistence;

public class RequestHandlingDal
{
    internal ApplicationDbContext context;

    public RequestHandlingDal(ApplicationDbContext context)
    {
        this.context = context;
    }

    
}