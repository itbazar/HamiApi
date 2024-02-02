using StackExchange.Redis;
using System.Text.Json;

namespace Application.Common.Interfaces.Maintenance;

public class MaintenanceService : IMaintenanceService
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private const string MainenanceKey = "maintenance_enabled";
    private const string TotalKey = "maintenance_total";
    private const string DoneKey = "maintenance_done";
    private const string ParametersKey = "maintenance_parameters";
    public MaintenanceService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }

    public async Task DisableMaitenanceModeAsync()
    {
        await setBool(MainenanceKey, false);
    }

    public async Task EnableMaitenanceModeAsync()
    {
        await setBool(MainenanceKey, true);
    }

    public async Task<bool> IsMaintenanceEnabledAsync()
    {
        return await getBool(MainenanceKey);
    }

    public async Task SetTotalAsync(long total)
    {
        await setLong(TotalKey, total);
    }

    public async Task<long> GetTotalAsync()
    {
        return await getLong(TotalKey);
    }

    public async Task SetDoneAsync(long done)
    {
        await setLong(DoneKey, done);
    }

    public async Task<long> AddDoneAsync(long value)
    {
        var done = await GetDoneAsync() + value;
        await setLong(DoneKey, done);
        return done;
    }

    public async Task<long> GetDoneAsync()
    {
        return await getLong(DoneKey);
    }


    private async Task setBool(string key, bool value)
    {
        await _database.StringSetAsync(key, value.ToString(), null);
    }

    private async Task<bool> getBool(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNull)
            return false;
        var t = bool.TryParse(value, out var enabled);
        return t && enabled;
    }

    private async Task setLong(string key, long value)
    {
        await _database.StringSetAsync(key, value.ToString(), null);
    }

    private async Task<long> getLong(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNull)
            return 0;
        var t = long.TryParse(value, out var readValue);
        return t == false ? 0 : readValue;
    }

    public async Task SetParametersAsync(ChangeInspectorKeyParameters parameters)
    {
        var value = JsonSerializer.Serialize(parameters);
        await _database.StringSetAsync(ParametersKey, value, null);
        await setLong(DoneKey, 0);
    }

    public async Task<ChangeInspectorKeyParameters?> GetParametersAsync()
    {
        string? serialized = await _database.StringGetAsync(ParametersKey);
        if (serialized is null)
            return null;
        var parameters = JsonSerializer.Deserialize<ChangeInspectorKeyParameters>(serialized);
        return parameters;
    }
}
