using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Matrix;

public class AutoValidationFilter : IActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public AutoValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do not run any argument checks if not PUT, PATCH, POST
        if (context.HttpContext.Request.Method != HttpMethod.Put.Method &&
            context.HttpContext.Request.Method != HttpMethod.Post.Method &&
            context.HttpContext.Request.Method != HttpMethod.Patch.Method)
        {
            return;
        }

        // If .net fails mapping to object, it returns an empty arguments list
        // Therefore, if the list is empty or if any of the arguments are null, the mapping failed and
        // The request is bad. (Only for PUT, PATCH, POST)
        if (context.ActionArguments.Any(arg => arg.Value == null) ||
            context.ActionArguments.Count == 0)
        {
            context.Result = new BadRequestObjectResult(
                new RequestDataError());
        }

        // Retreive the argument
        var parameter = context.ActionArguments.FirstOrDefault().Value;

        // Fail if cannot retreive parameter
        if (parameter == null)
            return;

        // Retreive the validator for the argument
        var validatorType = typeof(IValidator<>).MakeGenericType(parameter.GetType());
        IValidator validator = (_serviceProvider.GetService(validatorType) as IValidator)!;

        // Skip if no validators were found
        if (validator == null)
            return;

        // Validate
        var validationContext = new ValidationContext<object>(parameter);
        var validationResult = validator.Validate(validationContext);

        // On validation fail:
        if (!validationResult.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList())
            );
        }
    }
}
