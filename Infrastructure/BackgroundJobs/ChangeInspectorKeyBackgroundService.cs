using Application.Common.Interfaces.Maintenance;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs;

public class ChangeInspectorKeyBackgroundService : BackgroundService
{
    private readonly ILogger<ChangeInspectorKeyBackgroundService> _logger;
    public IServiceProvider Services { get; }

    public ChangeInspectorKeyBackgroundService(IServiceProvider services,
        ILogger<ChangeInspectorKeyBackgroundService> logger)
    {
        Services = services;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = 3;
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service running.");
            await DoWork(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(interval));
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using (var scope = Services.CreateScope())
        {
            var maintenanceService = scope.ServiceProvider.GetRequiredService<IMaintenanceService>();
            if (!await maintenanceService.IsMaintenanceEnabledAsync())
                return;

            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            var complaintRepository = scope.ServiceProvider.GetRequiredService<IComplaintRepository>();
            var publicKeyRepository = scope.ServiceProvider.GetRequiredService<IPublicKeyRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var parameters = await maintenanceService.GetParametersAsync();
            if (parameters is null)
            {
                await maintenanceService.DisableMaitenanceModeAsync();
                return;
            }

            List<Complaint> complaints;
            int skip = 0, take = 2;
            try
            {
                do
                {
                    var result = await complaintRepository.GetAsync(
                        c => c.PublicKeyId == parameters.FromKeyId, skip, take, true);
                    if (result.IsFailed)
                    {
                        throw new Exception();
                    }
                    complaints = result.Value;

                    foreach (var complaint in complaints)
                    {
                        if(complaint.ChangeInspectorKey(
                            parameters.FromPrivateKey,
                            parameters.ToPublicKey,
                            parameters.ToKeyId).IsFailed)
                            continue;
                    }
                    await unitOfWork.SaveAsync();

                    await maintenanceService.AddDoneAsync(complaints.Count);

                    await Task.Delay(TimeSpan.FromSeconds(10));

                } while (complaints.Any());

                await publicKeyRepository.SetActive(parameters.ToKeyId);
                await maintenanceService.DisableMaitenanceModeAsync();
            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await maintenanceService.DisableMaitenanceModeAsync();
                return;
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}