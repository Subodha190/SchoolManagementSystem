using SchoolManagement.API.Models;
using SchoolManagement.Application.Common.Constants;
using SchoolManagement.Application.Common.Exceptions;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var traceId = context.TraceIdentifier;

        var response = new SchoolManagement.API.Models.ErrorResponse
        {
            StatusCode = 500,
            Message = "An unexpected error occurred",
            Errors = null
        };

        switch (exception)
        {
            case SchoolManagement.Application.Common.Exceptions.AppValidationException validationException:
                context.Response.StatusCode = 400;
                response.StatusCode = 400;
                response.Message = "Validation Failed";
                response.Errors = validationException.Errors;
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = 401;
                response.StatusCode = 401;
                response.Message = "Unauthorized";
                break;

            case SchoolManagement.Application.Common.Exceptions.ForbiddenException:
                context.Response.StatusCode = 403;
                response.StatusCode = 403;
                response.Message = "Forbidden";
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = 404;
                response.StatusCode = 404;
                response.Message = exception.Message;
                break;

            case SchoolManagement.Application.Common.Exceptions.ConflictException:
                context.Response.StatusCode = 409;
                response.StatusCode = 409;
                response.Message = exception.Message;
                break;

            default:
                context.Response.StatusCode = 500;
                response.StatusCode = 500;
                response.Message = SchoolManagement.Application.Common.Constants.ErrorMessages.InternalServerError;
                break;
        }

        // Add trace & timestamp
        var wrapper = new
        {
            response.StatusCode,
            response.Message,
            Errors = response.Errors,
            Timestamp = DateTime.UtcNow,
            TraceId = traceId
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(wrapper));
    }
}