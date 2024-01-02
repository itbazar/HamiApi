using Domain.Models.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.ChartAggregate;

public class Chart : Entity
{
    private Chart(Guid id) : base(id) { }
    public int Order { get; set; }
    public int Code { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
    public long ValidForMilliseconds { get; set; }
    public bool IsDeleted {  get; set; } = false;
    public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
    public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    public static Chart Create(
        int code,
        string title,
        int order,
        long validForMilliseconds,
        string parameters,
        List<ApplicationRole> roles,
        List<ApplicationUser> users)
    {
        var chart = new Chart(Guid.Empty);
        chart.Code = code;
        chart.Title = title;
        chart.Order = order;
        chart.ValidForMilliseconds = validForMilliseconds;
        chart.Parameters = parameters;
        chart.Roles = roles;
        chart.Users = users;
        return chart;
    }

    public void Update(
        int? code,
        string? title,
        int? order,
        long? validForMilliseconds,
        string? parameters,
        List<ApplicationRole>? roles,
        List<ApplicationUser>? users)
    {
        Code = code ?? Code;
        Title = title ?? Title;
        Order = order ?? Order;
        ValidForMilliseconds = validForMilliseconds ?? ValidForMilliseconds;
        Parameters = parameters ?? Parameters;
        Roles = roles ?? Roles;
        Users = users ?? Users;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
}
