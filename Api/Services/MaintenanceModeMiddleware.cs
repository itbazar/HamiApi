using Application.Common.Interfaces.Maintenance;

namespace Api.Services;

public class MaintenanceModeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _allowedPaths;
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceModeMiddleware(RequestDelegate next, IMaintenanceService maintenanceService)
    {
        _next = next;
        _maintenanceService = maintenanceService;
        _allowedPaths = new List<string>() { "/api/admin/changekey" };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(_maintenanceService.IsMaintenanceEnabled())
        {
            if(_allowedPaths.Contains(context.Request.Path.ToString().ToLower()))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.StartAsync();
            }
        }
        await _next(context);
    }
}
