using BuildingBlocks.Common.Interfaces;
using BuildingBlocks.Infrastructure.Interceptors;
using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Repositories;
using BuildingBlocks.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the service collection with audit and soft delete interceptors.
    /// </summary>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <param name="currentUserId">The current user identifier for audit purposes.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddInfrastructure<TContext>(this IServiceCollection services, string connectionString, string? currentUserId = null)
        where TContext : BaseDbContext
    {
        services.AddDbContext<TContext>(options =>
            options.UseSqlServer(connectionString)
                   .AddInterceptors(new AuditInterceptor(currentUserId), new SoftDeleteInterceptor(currentUserId)));

        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

        // Register repository factory
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }

    /// <summary>
    /// Adds MediatR services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">The assemblies to scan for handlers.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddMediatRInfrastructure(this IServiceCollection services, params System.Reflection.Assembly[] assemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
        });

        return services;
    }
}