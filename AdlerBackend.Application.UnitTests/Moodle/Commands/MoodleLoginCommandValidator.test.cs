using AdLerBackend.Application.Moodle.Commands;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Moodle.Commands;

public class MoodleLoginCommandValidator_test
{
    private GetMoodleUserDataCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetMoodleUserDataCommandValidator();
    }

    [Test]
    public void Should_have_error_when_username_is_null()
    {
        var command = new GetMoodleUserDataCommand
        {
            UserName = null,
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_username_is_empty()
    {
        var command = new GetMoodleUserDataCommand
        {
            UserName = "",
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_null()
    {
        var command = new GetMoodleUserDataCommand
        {
            UserName = "username",
            Password = null
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_empty()
    {
        var command = new GetMoodleUserDataCommand
        {
            UserName = "username",
            Password = ""
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }
}