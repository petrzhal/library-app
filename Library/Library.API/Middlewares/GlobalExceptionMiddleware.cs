using Library.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Library.API.Middlewares
{
    public class GlobalExceptionMiddleware(ILoggerFactory loggerFactory) : IMiddleware
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<GlobalExceptionMiddleware>();

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception caught in GlobalExceptionMiddleware: {exception}", ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            var response = new
            {
                message = "An unexpected error occurred.",
                details = (object?)null
            };

            switch (exception)
            {
                case ValidationAppException validationAppException:
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    response = new
                    {
                        message = "Validation failed.",
                        details = validationAppException.Errors
                            .GroupBy(
                                e => e.Key,
                                e => e.Value,
                                (propertyName, errorMessages) => new
                                {
                                    Property = propertyName,
                                    Messages = errorMessages.SelectMany(msg => msg).Distinct().ToList()
                                }
                            ) as object
                    };
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new
                    {
                        message = "Unauthorized access.",
                        details = exception.Message as object
                    };
                    break;

                case InvalidCredentialsException:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new
                    {
                        message = "Incorrect username or password.",
                        details = (object?)null
                    };
                    break;

                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new
                    {
                        message = exception.Message,
                        details = (object?)null
                    };
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new
                    {
                        message = "Resource not found.",
                        details = exception.Message as object
                    };
                    break;

                case EntityNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new
                    {
                        message = "Entity not found.",
                        details = exception.Message as object
                    };
                    break;

                case BookAlreadyBorrowedException:
                    statusCode = HttpStatusCode.Conflict;
                    response = new
                    {
                        message = "The book is already borrowed.",
                        details = (object?)null
                    };
                    break;

                default:
                    response = new
                    {
                        message = "An unexpected error occurred.",
                        details = exception.Message as object
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var responseBody = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(responseBody);
        }
    }
}
