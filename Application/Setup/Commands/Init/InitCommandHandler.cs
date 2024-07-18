using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Models.WebContents;
using SharedKernel.Statics;

namespace Application.Setup.Commands.Init;

internal class InitCommandHandler(
    IComplaintCategoryRepository categoryRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPublicKeyRepository publicKeyRepository,
    IComplaintOrganizationRepository organizationRepository,
    IChartRepository chartRepository,
    IWebContentRepository webContentRepository) : IRequestHandler<InitCommand, Result<string>>
{
public async Task<Result<string>> Handle(InitCommand request, CancellationToken cancellationToken)
    {
        await initCategories();
        await initOrganizations();
        await initRolesAndUsers();
        await initCharts();
        await initWebContents();
        var privateKey = await initPublicKey();
        return privateKey;
    }

    private async Task initCategories()
    {
        var complaintCategories = await categoryRepository.GetAsync();
        if (complaintCategories is not null && complaintCategories.Count() > 0)
        {
            return;
        }
        var titles = new List<string>()
        {
            "رشاء",
            "ارتشاء",
            "سایر"
        };

        foreach (var title in titles)
        {
            categoryRepository.Insert(ComplaintCategory.Create(title, ""));
        }

        await unitOfWork.SaveAsync();
    }

    private async Task initOrganizations()
    {
        var complaintOrganizations = await organizationRepository.GetAsync();
        if (complaintOrganizations is not null && complaintOrganizations.Count() > 0)
        {
            return;
        }
        var titles = new List<string>()
        {
            "شورای شهر",
            "عمران",
            "سایر"
        };

        foreach (var title in titles)
        {
            organizationRepository.Insert(ComplaintOrganization.Create(title, ""));
        }

        await unitOfWork.SaveAsync();
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

    
    private async Task<string> initPublicKey()
    {
        if (unitOfWork.DbContext.Set<PublicKey>().Any())
            return "Already initialized.";
        var inspector = (await userRepository.GetUsersInRole("Inspector")).FirstOrDefault();
        if(inspector is null)
        {
            throw new Exception("No inspector found.");
        }
        var keyPair = PublicKey.GenerateKeyPair();
        var publicKey = PublicKey.Create("Initial", keyPair.PublicKey, inspector.Id, true);
        await publicKeyRepository.Add(publicKey);
        return keyPair.PrivateKey;
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

    private async Task initWebContents()
    {
        var initialWebContents = new List<WebContent>
        {
            WebContent.Create("About", "", "<H1>درباره ما</H1>"),
            WebContent.Create("Contanct", "", "<H1>تماس با ما</H1>"),
            WebContent.Create("PreRequest", "", "<H1>لطفاً گزارش خود را ثبت کنید.</H1>"),
            WebContent.Create("PostRequest", "", "<H1>با تشکر از شما</H1>")
        };
        var webContents = await webContentRepository.GetAsync();
        foreach (var webContent in initialWebContents)
        {
            if (!webContents.Any(wc => wc.Title == webContent.Title))
            {
                webContentRepository.Insert(webContent);
            }
        }
        
        await unitOfWork.SaveAsync();
    }
}
