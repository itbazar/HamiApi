using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using Domain.Models.IdentityAggregate;
using SharedKernel.Statics;

namespace Application.Setup.Commands.Init;

internal class InitCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IChartRepository chartRepository) : IRequestHandler<InitCommand, Result<string>>
{
public async Task<Result<string>> Handle(InitCommand request, CancellationToken cancellationToken)
    {
        await initRolesAndUsers();
        await initCharts();
        return "ok";
    }

    private async Task initRolesAndUsers()
    {
        //Init roles
        var rolesInfo = new List<Tuple<string, string>>()
        {
            new("Citizen", "شهروند"),
            new("Admin", "مدیر فنی"),
            new("PowerUser",""),
            new("Inspector", "بازرس")
        };


        var roles = new List<ApplicationRole>();
        foreach (var role in rolesInfo)
        {
            if (!await userRepository.RoleExistsAsync(role.Item1))
            {
                var r = new ApplicationRole() { Name = role.Item1, Title = role.Item2 };
                await userRepository.CreateRoleAsync(r);
                roles.Add(r);
            }
        }

        //Init users
        //TODO: exclude non-actor users like poweruser, admin, ...
        var usersInfo = new List<Tuple<string, string, string>>() {
            new("Admin", "Admin", "مدیر فنی"),
            new("PowerUser", "PowerUser", ""),
            new("Inspector", "Inspector", "بازرس")
        };

        ApplicationUser u;
        var users = new List<ApplicationUser>();

        foreach (var user in usersInfo)
        {
            if (await userRepository.FindByNameAsync(user.Item2) == null)
            {
                u = new ApplicationUser()
                {
                    UserName = user.Item2,
                    Title = user.Item3,
                    PhoneNumber = "09137473007",
                    PhoneNumberConfirmed = true
                };

                await userRepository.CreateAsync(u, "aA@12345");

                await userRepository.AddToRoleAsync(u, user.Item1);
                users.Add(u);
            }
        }
    }
    private async Task initCharts()
    {
        if (unitOfWork.DbContext.Set<Chart>().Any())
            return;
        var allRoles = await userRepository.GetRoles();
        var adminRole = allRoles.Where(r => r.Name == RoleNames.Admin).First();
        var inspectorRole = allRoles.Where(r => r.Name == RoleNames.Inspector).First();
        var users = new List<ApplicationUser>();
        Chart chart;
        chart = Chart.Create(
            ChartCodes.Dashboard,
            "داشبورد مدیریت",
            1,
            1,
            "",
            new List<ApplicationRole>() { adminRole, inspectorRole},
            users);
        chart.Delete(true);
        chartRepository.Insert(chart);

        chart = Chart.Create(
            ChartCodes.ComplaintCategoryHistogram,
            "فراوانی گزارش ها بر اساس دسته بندی",
            2,
            1,
            "",
            new List<ApplicationRole>() { adminRole, inspectorRole },
            users);
        chartRepository.Insert(chart);

        chart = Chart.Create(
            ChartCodes.ComplaintOrganizationHistogram,
            "فراوانی گزارش ها بر اساس واحد مربوطه",
            3,
            1,
            "",
            new List<ApplicationRole>() { adminRole, inspectorRole },
            users);
        chartRepository.Insert(chart);

        chart = Chart.Create(
            ChartCodes.ComplaintStatusHistogram,
            "فراوانی گزارش ها بر اساس وضعیت",
            4,
            1,
            "",
            new List<ApplicationRole>() { adminRole, inspectorRole },
            users);
        chartRepository.Insert(chart);

        await unitOfWork.SaveAsync();
    }
}
