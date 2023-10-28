using DataAccess.Contracts;
using Domain.Core.Friendship;
using Domain.Core.FriendshipRequest;
using Domain.Core.Image;
using Domain.Core.User;
using Infrastructure.DataAccess.Configuration;
using Infrastructure.DataAccess.Context;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> action,
        IConfiguration configuration)
    {
        services.Configure<ImageStorageConfiguration>(configuration.GetSection(ImageStorageConfiguration.SectionKey));

        services.AddDbContext<IDatabaseContext, DatabaseContext>(action);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IFriendshipRequestRepository, FriendshipRequestRepository>();
        services.AddScoped<ImageRepository>();
        services.AddScoped<IImageRepository>(x =>
            new LocalImageRepository(
                x.GetRequiredService<ImageRepository>(),
                x.GetRequiredService<IOptions<ImageStorageConfiguration>>()));

        return services;
    }

    public static Task MigrateAsync(this IServiceProvider provider)
    {
        var context = provider.GetRequiredService<DatabaseContext>();

        return context.Database.MigrateAsync();
    }
}