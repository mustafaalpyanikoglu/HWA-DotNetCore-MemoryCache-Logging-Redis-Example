using System.Reflection;
using Application.Services.CategoryServices;
using Application.Services.ProductServices;
using Application.Services.Repositories;
using Core.Application.Caching;
using Core.Application.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IProductService,ProductManager>();
        services.AddScoped<ICategoryService,CategoryManager>();
        
        services.AddSingleton<LoggerServiceBase, FileLogger>();

        return services;

    }
    
    
}