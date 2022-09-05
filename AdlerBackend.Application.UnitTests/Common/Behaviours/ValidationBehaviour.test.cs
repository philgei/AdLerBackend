using AdLerBackend.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using NSubstitute;
using ValidationException = AdLerBackend.Application.Common.Exceptions.ValidationException;

namespace AdLerBackend.Application.UnitTests.Common.Behaviours;

public class TestModel : IRequest<string>
{
    public string Test1 { get; set; }
    public int Test2 { get; set; }
}

public class TestModelValidator : AbstractValidator<TestModel>
{
    public TestModelValidator()
    {
        RuleFor(x => x.Test1).NotEmpty();
        RuleFor(x => x.Test2).NotEmpty();
        RuleFor(x => x.Test2).GreaterThan(5);
    }
}

public class ValidationBehaviourTest
{
    [Test]
    public Task ValidationBehaviour_Valid_ShouldThrowWithErrors()
    {
        // Arrange
        var systemUnderTest =
            new ValidationBehaviour<TestModel, string>(
                new List<IValidator<TestModel>>
                {
                    new TestModelValidator()
                });

        // Act
        Assert.ThrowsAsync<ValidationException>(async () =>
            await systemUnderTest.Handle(new TestModel
                {
                    Test2 = 2
                }, new CancellationToken(),
                Substitute.For<RequestHandlerDelegate<string>>()));
        return Task.CompletedTask;
    }

    [Test]
    public async Task ValidationBehaviour_Valid_ShouldNotThrow()
    {
        // Arrange
        var systemUnderTest =
            new ValidationBehaviour<TestModel, string>(
                new List<IValidator<TestModel>>
                {
                    new TestModelValidator()
                });

        // Act
        await systemUnderTest.Handle(new TestModel
            {
                Test1 = "test",
                Test2 = 6
            }, new CancellationToken(),
            Substitute.For<RequestHandlerDelegate<string>>());
    }
}