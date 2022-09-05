using AdLerBackend.Application.Common.Exceptions;
using FluentValidation.Results;

namespace AdLerBackend.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTest
{
    [Test]
    public void Constructor_WhenCalled_ShouldSetProperties()
    {
        // Arrange
        var errors = new List<ValidationFailure>
        {
            new("prop1", "error1"),
            new("prop2", "error2")
        };
        // Act
        var exceptionUnderTest = new ValidationException(errors);

        // Assert
        Assert.That(exceptionUnderTest.Errors, Is.EquivalentTo(errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray())));
    }
}