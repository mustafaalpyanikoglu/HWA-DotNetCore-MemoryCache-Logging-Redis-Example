using System.Reflection;
using Application.Services.CategoryServices;
using Application.Services.ProductServices;
using Core.Application.Caching;
using Core.Application.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Provides extension methods for registering application services in the dependency injection container.
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Registers application services in the dependency injection container.
    /// </summary>
    /// <param name="services">The collection of service descriptors.</param>
    /// <returns>The collection of service descriptors.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registering MediatR with open behaviors
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Registering application services
        services.AddScoped<IProductService, ProductManager>();
        services.AddScoped<ICategoryService, CategoryManager>();
        
        // Registering logger service
        // services.AddSingleton<LoggerServiceBase, FileLogger>();
        services.AddSingleton<LoggerServiceBase, MsSqlLogger>();

        return services;
    }
}