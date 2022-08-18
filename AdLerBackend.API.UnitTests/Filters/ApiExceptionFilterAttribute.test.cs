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
    [SetUp]
    public void Setup()
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        _context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
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
        var result = _context.Result as UnauthorizedObjectResult;
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
        Assert.That(_context.Result, Is.InstanceOf<BadRequestObjectResult>());
        var result = (BadRequestObjectResult) _context.Result!;

        Assert.That(result.Value, Is.InstanceOf<ValidationProblemDetails>());
        var resultValue = (ValidationProblemDetails) result.Value!;

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
        var resultValue = result!.Value as ProblemDetails;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
        Assert.That(resultValue!.Title, Is.EqualTo("An unknown error occurred while processing your request."));
    }

    [Test]
    public void ApiExceptionFilterAttributeShouldHandleTokenException()
    {
        // Arrange
        _context.Exception = new InvalidTokenException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as UnauthorizedObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<MoodleTokenProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_NotFoundException_ShouldReturnProblemDetails()
    {
        // Arrange
        _context.Exception = new NotFoundException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as NotFoundObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_ForbiddenAccessException_ShouldReturnNotFoundObjectResult()
    {
        // Arrange
        _context.Exception = new ForbiddenAccessException("Test");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as UnauthorizedObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }
    
    [Test]
    public void ApiExceptionFilterAttribute_CourseCreationException()
    {
        // Arrange
        _context.Exception = new CourseCreationException("Test");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ConflictObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

#pragma warning disable CS8618
    private ExceptionContext _context;
    private ApiExceptionFilterAttribute _filter;
#pragma warning restore CS8618
}