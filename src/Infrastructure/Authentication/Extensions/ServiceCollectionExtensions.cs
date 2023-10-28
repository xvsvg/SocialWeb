using System.Text;
using DataAccess.Contracts;
using Domain.Core.Services;
using Infrastructure.Authentication.Cryptography;
using Infrastructure.Authentication.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHashChecker, PasswordHasher>();
        services.AddHttpContextAccessor();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[$"{nameof(JwtSettings)}:Issuer"],
                ValidAudience = configuration[$"{nameof(JwtSettings)}:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration[$"{nameof(JwtSettings)}:SecurityKey"]!))
            });

        return services;
    }
}