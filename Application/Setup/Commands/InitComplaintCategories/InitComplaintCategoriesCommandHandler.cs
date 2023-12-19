using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Setup.Commands.InitComplaintCategories;

internal class InitComplaintCategoriesCommandHandler : IRequestHandler<InitComplaintCategoriesCommand, bool>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InitComplaintCategoriesCommandHandler(
        IComplaintCategoryRepository categoryRepository,
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(InitComplaintCategoriesCommand request, CancellationToken cancellationToken)
    {
        await initCategories();
        await initRolesAndUsers();
        return true;
    }

    private async Task initCategories()
    {
        var complaintCategories = await _categoryRepository.GetAsync();
        if (complaintCategories is not null && complaintCategories.Count() > 0)
        {
            return;
        }
        var titles = new List<string>()
        {
            "رشاء",
            "ارتشاء",
        };

        foreach (var title in titles)
        {
            _categoryRepository.Insert(ComplaintCategory.Create(title, ""));
        }

        await _unitOfWork.SaveAsync();
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
            if (!await _userRepository.RoleExistsAsync(role.Item1))
            {
                var r = new ApplicationRole() { Name = role.Item1, Title = role.Item2 };
                await _userRepository.CreateRoleAsync(r);
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
            if (await _userRepository.FindByNameAsync(user.Item2) == null)
            {
                u = new ApplicationUser()
                {
                    UserName = user.Item2,
                    Title = user.Item3,
                    PhoneNumberConfirmed = true
                };

                await _userRepository.CreateAsync(u, "aA@12345");

                await _userRepository.AddToRoleAsync(u, user.Item1);
                users.Add(u);
            }
        }
    }
}
