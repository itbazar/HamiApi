using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Services.Filters;

public class LoggingFilter : IActionFilter

{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //Console.WriteLine("..............This filter Executed on : OnActionExecuting................ ");

        var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
        var requestPath = context.HttpContext.Request.Path;
        var method = context.HttpContext.Request.Method;
        var actionArguments = context.ActionArguments;
        var accessTime = DateTime.UtcNow;

        _logger.LogInformation("IP {ClientIpAddress} Accessed to {Method} {requestPath} in {accessTime} \n With Payload : {actionArguments} ",
            ipAddress,
            method,
            requestPath,
            accessTime,
            actionArguments);

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //Console.WriteLine("..............This filter Executed on : OnActionExecuted................ ");
    }

}
