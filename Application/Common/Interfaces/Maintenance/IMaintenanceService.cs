namespace Application.Common.Interfaces.Maintenance;

public interface IMaintenanceService
{
    Task EnableMaitenanceModeAsync();
    Task DisableMaitenanceModeAsync();
    Task<bool> IsMaintenanceEnabledAsync();
    bool IsMaintenanceEnabled();
    Task SetTotalAsync(long total);
    Task<long> GetTotalAsync();
    Task<long> AddDoneAsync(long value);
    Task<long> AddFailedAsync(long value);
    Task SetDoneAsync(long done);
    Task<long> GetDoneAsync();
    Task<long> GetFailedAsync();
    Task SetParametersAsync(ChangeInspectorKeyParameters parameters);
    Task<ChangeInspectorKeyParameters?> GetParametersAsync();
}

public record ChangeInspectorKeyParameters(
    Guid FromKeyId,
    string FromPrivateKey,
    string ToPublicKey,
    Guid ToKeyId,
    long Total);