using Api;
using Api.Services;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration);

var maxFileSize = builder.Configuration.GetSection("Storage")
            .GetSection("MaxFileSize")
            .Get<long>();
builder.WebHost.ConfigureKestrel(options => { options.Limits.MaxRequestBodySize = maxFileSize; });
//Logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Perform migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
//app.UseMiddleware<MaintenanceModeMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
//app.UseAccessControlMiddleware();
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseSerilogRequestLogging();
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Running in Development environment");
    app.UseDeveloperExceptionPage();
}
else
{
    Console.WriteLine("Running in Non-Development environment");
    app.UseExceptionHandler();
}
//app.UseExceptionHandler();
app.UseStaticFiles();
app.MapControllers();


app.Run();
