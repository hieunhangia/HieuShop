using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace API.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BaseException baseException) return false;

        switch (baseException)
        {
            case BadRequestException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = baseException.Title,
                    Detail = baseException.Detail
                }, cancellationToken);
                break;

            case UnauthorizedAccessException:
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = baseException.Title,
                    Detail = baseException.Detail
                }, cancellationToken);
                break;

            case ForbiddenAccessException:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = baseException.Title,
                    Detail = baseException.Detail
                }, cancellationToken);
                break;

            case NotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = baseException.Title,
                    Detail = baseException.Detail
                }, cancellationToken);
                break;

            case ValidationException validationException:
                httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(validationException.Errors)
                {
                    Title = baseException.Title,
                    Detail = baseException.Detail
                }, cancellationToken);
                break;
        }

        return true;
    }
}