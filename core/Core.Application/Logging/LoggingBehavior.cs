using System.Text.Json;
using Core.CrossCuttingConcerns.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Application.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        List<LogParameter> logParameters = [new LogParameter { Type = request.GetType().Name, Value = request }];

        LogDetail logDetail =
            new()
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"
            };

        _logger.LogInformation(JsonSerializer.Serialize(logDetail));
        return await next();
    }
}
