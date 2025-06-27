using FarmProject.Application.Common;
using FarmProject.Application.IdentityService;
using FarmProject.Application.IdentityService.Validators;
using FarmProject.Infrastructure.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FarmProject.API.DependencyInjection;

public static class AuthServiceCollectionExtension
{
    public static IServiceCollection AddSwaggerJwtAuth(this IServiceCollection services)
    {
        services.ConfigureSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                },
            });
        });
        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        // Register the service for creating JWT tokens
        services.AddScoped<IIdentityService, IdentityService>();

        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        // Add authentication with JWT Bearer
        services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

        .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtSettings?.SigningKey ?? 
                            throw new InvalidOperationException("JWT signing key not configured"))),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audiences,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                jwt.Audience = jwtSettings.Audiences?[0];
                jwt.ClaimsIssuer = jwtSettings.Issuer;
            });

        return services;
    }

    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        // Add Identity Core
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService>(provider =>
        {
            var baseService = new UserService(
                provider.GetRequiredService<UserManager<IdentityUser>>(),
                provider.GetRequiredService<RoleManager<IdentityRole>>(),
                provider.GetRequiredService<SignInManager<IdentityUser>>(),
                provider.GetRequiredService<IIdentityService>());

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationUserService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingUserService(validatedService, loggingHelper);
        });

        return services;
    }

    public static IServiceCollection AddIdentityValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterUserParam>, RegisterUserParamValidator>();
        services.AddScoped<IValidator<LoginUserParam>, LoginUserParamValidator>();

        return services;
    }
}