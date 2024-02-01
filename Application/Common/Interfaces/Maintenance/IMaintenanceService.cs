namespace Application.Common.Interfaces.Maintenance;

public interface IMaintenanceService
{
    Task EnableMaitenanceModeAsync();
    Task DisableMaitenanceModeAsync();
    Task<bool> IsMaintenanceEnabledAsync();
    Task SetTotalAsync(long total);
    Task<long> GetTotalAsync();
    Task AddDoneAsync(long done);
    Task SetDoneAsync(long done);
    Task<long> GetDoneAsync();
}
