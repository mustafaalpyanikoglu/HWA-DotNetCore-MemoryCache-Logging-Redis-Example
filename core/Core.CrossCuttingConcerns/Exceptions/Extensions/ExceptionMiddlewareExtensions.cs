using Microsoft.AspNetCore.Builder;

namespace Core.CrossCuttingConcerns.Exceptions.Extensions;

public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Extension method to configure the custom exception middleware in the application pipeline.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance.</param>
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();
}