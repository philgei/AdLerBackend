using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Common.ProblemDetails;
using AdLerBackend.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AdLerBackend.API.Test.Filters;

public class ApiExceptionFilterAttributeTest
{
    private ExceptionContext _context = null!;
    private ApiExceptionFilterAttribute _filter = null!;

    [SetUp]
    public void Setup()
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        _context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = new InvalidMoodleLoginException("______Invalid Login!")
        };
        _filter = new ApiExceptionFilterAttribute();
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_InvalidMoodleLogin()
    {
        // Arrange
        _context.Exception = new InvalidMoodleLoginException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as BadRequestObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<MoodleLoginProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_ValidationException()
    {
        // Arrange
        _context.Exception = new ValidationException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as BadRequestObjectResult;
        var resultValue = result.Value as ValidationProblemDetails;

        Assert.IsInstanceOf<ValidationProblemDetails>(resultValue);
        Assert.That(resultValue.Type, Is.EqualTo("Validation Error"));
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_UnknownException()
    {
        // Arrange
        _context.Exception = new Exception("Unknown Exception");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result.Value as ProblemDetails;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
        Assert.That(resultValue.Title, Is.EqualTo("An unknown error occurred while processing your request."));
    }
}