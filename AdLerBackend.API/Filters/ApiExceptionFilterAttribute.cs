using AdLerBackend.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdLerBackend.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(ValidationException), HandleValidationException},
            {typeof(InvalidMoodleLoginException), HandleMoodleLoginException}
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
            _exceptionHandlers[type].Invoke(context);
        else
            HandleUnknownException(context);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Title = "An unknown error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException) context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "Validation Error"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleMoodleLoginException(ExceptionContext context)
    {
        var exception = (InvalidMoodleLoginException) context.Exception;

        var details = new ProblemDetails
        {
            Title = "Invalid Moodle Login",
            Detail = exception.Message,
            Status = StatusCodes.Status400BadRequest
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}