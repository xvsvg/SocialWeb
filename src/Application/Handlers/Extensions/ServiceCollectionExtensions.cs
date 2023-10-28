using Application.Core;
using Application.Core.Behaviours;
using Application.Core.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Handlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PaginationConfiguration>(configuration.GetSection(PaginationConfiguration.SectionKey));
        services.AddValidatorsFromAssembly(typeof(IApplicationHandlersMarker).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationHandlersMarker>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationCoreMarker>());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}