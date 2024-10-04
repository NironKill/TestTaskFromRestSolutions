using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OrderManagement.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest
        : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Guid correlationId = Guid.NewGuid();

            string requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation("Type request:{RequestName}, id:{CorrelationID}, data:{Request}", typeof(TRequest).Name, correlationId, requestJson);

            TResponse response = await next();

            string responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation("Type response:{ResponseName}, id:{CorrelationID}, data:{Response}", typeof(TResponse).Name, correlationId, responseJson);

            return response;
        }
    }
}
