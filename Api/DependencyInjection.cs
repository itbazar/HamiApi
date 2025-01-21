using Api.Services;
using Domain.Models.IdentityAggregate;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        //services.AddControllers()
        //    .AddJsonOptions(options =>
        //        {
        //             options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        //             options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        //        });


        services.AddSwagger();
        services.AddCors(configuration);
        services.AddIdentification(configuration);

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            //This is to generate the Default UI of Swagger Documentation    
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Hami Web API",
                Description = ""
            });
            // To Enable authorization using Swagger (JWT)    
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
        });

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                b =>
                {
                    b
                        .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>() ?? throw new Exception("Allowd origins cannot be null."))
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .WithExposedHeaders("X-Pagination", "Captcha-Key")
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
        return services;
    }

    public static IServiceCollection AddIdentification(this IServiceCollection services, IConfiguration configuration)
    {
        //For Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
            .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        //Adding Authentication
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:Audience"],
            ValidIssuer = configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new Exception("Jwt secret cannot be null."))),
            ClockSkew = TimeSpan.Zero
        };
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            //Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });

        return services;
    }
}
