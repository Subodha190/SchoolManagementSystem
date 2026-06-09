using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolManagement.Application.Common.Exceptions;

public class ValidationFilter<T>
    : IAsyncActionFilter
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(
        IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var argument =
            context.ActionArguments.Values
            .OfType<T>()
            .FirstOrDefault();

        if (argument != null)
        {
            var result =
                await _validator.ValidateAsync(argument);

            if (!result.IsValid)
            {
                throw new AppValidationException(
                    result.Errors.Select(x => x.ErrorMessage));
            }
        }

        await next();
    }
}