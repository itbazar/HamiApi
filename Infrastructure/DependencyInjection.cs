using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Maintenance;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Captcha;
using Infrastructure.Communications;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Storage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration.GetConnectionString("DefaultConnection"));
        services.AddRedis(configuration);
        services.AddRepositories();
        services.AddSecurity(configuration);
        services.AddEncryption();
        services.AddStorage(configuration);
        services.AddCommunication(configuration);

        services.AddHostedService<ChangeInspectorKeyBackgroundService>();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        if (connectionString is null)
            throw new Exception("Connection string cannot be null.");
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                o =>
                {
                    //o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    o.MigrationsAssembly("Infrastructure");
                }));
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
                 ConnectionMultiplexer.Connect(new ConfigurationOptions
                 {
                     EndPoints = { $"{configuration.GetValue<string>("Redis:Host")}:{configuration.GetValue<int>("Redis:Port")}" },
                     AbortOnConnectFail = false,
                 }));
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IComplaintRepository, ComplaintRepository>();
        services.AddScoped<IComplaintCategoryRepository, ComplaintCategoryRepository>();
        services.AddScoped<IComplaintOrganizationRepository, ComplaintOrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommunicationService, CommunicationServiceUsingMessageBroker>();
        services.AddScoped<IPublicKeyRepository, PublicKeyRepository>();
        services.AddScoped<IChartRepository, ChartRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IWebContentRepository, WebContentRepository>();
        services.AddSingleton<ICaptchaProvider, SixLaborsCaptchaProvider>();
        services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
        services.AddScoped<IMaintenanceService, MaintenanceService>();
        services.AddScoped<IDiseaseRepository, DiseaseRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<IPatientGroupRepository, PatientGroupRepository>();
        services.AddScoped<ITestPeriodRepository, TestPeriodRepository>();
        services.AddScoped<ITestPeriodResultRepository, TestPeriodResultRepository>();
        services.AddScoped<ICounselingSessionRepository, CounselingSessionRepository>();
        services.AddScoped<IUserGroupMembershipRepository, UserGroupMembershipRepository>();
        services.AddScoped<IUserMedicalInfoRepository, UserMedicalInfoRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();

        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtInfo = configuration.GetSection("JWT").Get<JwtInfo>();
        if (jwtInfo is null)
            throw new Exception("Jwt info not exist");
        
        services.AddSingleton(jwtInfo);
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICaptchaProvider, SixLaborsCaptchaProvider>();

        return services;
    }

    public static IServiceCollection AddEncryption(this IServiceCollection services)
    {
        //services.AddScoped<ISymmetricEncryption, AesEncryption>();
        //services.AddScoped<IAsymmetricEncryption, RsaEncryption>();
        //services.AddScoped<IHasher, Hasher>();

        return services;
    }

    public static IServiceCollection AddStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<StorageOptions>(
            configuration.GetSection(StorageOptions.Name));
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }

    public static IServiceCollection AddCommunication(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        string host = configuration["MessageBroker:Host"] ?? throw new Exception("Message broker configurations not found");
        string username = configuration["MessageBroker:Username"]! ?? throw new Exception("Message broker configurations not found");
        string password = configuration["MessageBroker:Password"]! ?? throw new Exception("Message broker configurations not found");
        // Add MassTransit as a service
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                //configurator.Message<MessageBrokerMessage>(x =>
                //{
                //    x.SetEntityName("shahrbin-communication");
                //});
                configurator.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ICommunicationService, CommunicationServiceUsingMessageBroker>();
        //services.AddSingleton<ISmsService>(x => new KaveNegarSms(
        //    new KaveNegarInfo(
        //        "10008000600033",
        //        "6367746F52314D6A52574C4E5766372F76653278365466334B6F777A35463764732F765667653332396F593D",
        //        "Namay")));

        return services;
    }

}
