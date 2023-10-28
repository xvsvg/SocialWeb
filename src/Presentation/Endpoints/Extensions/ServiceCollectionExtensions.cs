using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer().AddAuthorization().AddFastEndpoints().SwaggerDocument();

        return services;
    }
}