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
        context.Response.ContentType =
            "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case AppValidationException validationException:

                context.Response.StatusCode = 400;

                response.StatusCode = 400;

                response.Message =
                    "Validation Failed";

                response.Errors =
                    validationException.Errors;

                break;

            case KeyNotFoundException:

                context.Response.StatusCode = 404;

                response.StatusCode = 404;

                response.Message =
                    exception.Message;

                break;

            default:

                context.Response.StatusCode = 500;

                response.StatusCode = 500;

                response.Message =
                    ErrorMessages.InternalServerError;

                break;
        }

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}