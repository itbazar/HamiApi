using Application.Common.Interfaces.Encryption;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using MediatR;

namespace Application.Setup.Commands.Init;

internal class InitCommandHandler : IRequestHandler<InitCommand, string>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsymmetricEncryption _asymmetric;
    private readonly IPublicKeyRepository _publicKeyRepository;

    public InitCommandHandler(
        IComplaintCategoryRepository categoryRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAsymmetricEncryption asymmetric,
        IPublicKeyRepository publicKeyRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _asymmetric = asymmetric;
        _publicKeyRepository = publicKeyRepository;
    }

    public async Task<string> Handle(InitCommand request, CancellationToken cancellationToken)
    {
        await initCategories();
        await initRolesAndUsers();
        var privateKey = await initPublicKey();
        return privateKey;
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

    private async Task<string> initPublicKey()
    {
        var inspector = (await _userRepository.GetUsersInRole("Inspector")).FirstOrDefault();
        if(inspector is null)
        {
            throw new Exception("No inspector found.");
        }
        var keyPair = _asymmetric.Generate();
        var publicKey = PublicKey.Create(keyPair.PublicKey, inspector.Id);
        await _publicKeyRepository.Add(publicKey);
        return keyPair.PrivateKey;
    }
}
