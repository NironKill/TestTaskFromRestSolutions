using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Enums;

namespace OrderManagement.API.Handlers
{
    public class HandlerException : IExceptionHandler
    {
        private readonly ILogger<HandlerException> _logger;

        public HandlerException(ILogger<HandlerException> logger) => _logger = logger;
        
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;

            if (exception is ValidationException fluentException)
            {
                ValidationFailure error = fluentException.Errors.FirstOrDefault();
                switch (int.Parse(error.ErrorCode))
                {
                    case (int)StatusCode.NotFound:
                        problemDetails.Detail = error.ErrorMessage;
                        problemDetails.Title = "The server can not find the requested resource.";
                        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case (int)StatusCode.BadRequest:
                        problemDetails.Detail = error.ErrorMessage;
                        problemDetails.Title = "The server could not understand the request due to incorrect syntax.";
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    default:
                        problemDetails.Detail = error.ErrorMessage;
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        problemDetails.Title = "The server encountered an unexpected condition that prevented it from fulfilling the request.";
                        break;
                }

                List<string> validationErrors = new List<string>();
                foreach (ValidationFailure otherError in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                problemDetails.Extensions.Add("other errors", validationErrors);
            }

            else
            {
                problemDetails.Title = exception.Message;
            }

            _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
