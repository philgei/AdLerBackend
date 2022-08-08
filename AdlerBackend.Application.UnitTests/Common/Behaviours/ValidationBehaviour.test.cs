using AdLerBackend.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using NSubstitute;
using ValidationException = AdLerBackend.Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.Common.Behaviours;

public class TestModel : IRequest<string>
{
    public string test1 { get; set; }
    public int test2 { get; set; }
}

public class TestModelValidator : AbstractValidator<TestModel>
{
    public TestModelValidator()
    {
        RuleFor(x => x.test1).NotEmpty();
        RuleFor(x => x.test2).NotEmpty();
        RuleFor(x => x.test2).GreaterThan(5);
    }
}

public class ValidationBehaviourTest
{
    [Test]
    public async Task ValidationBehaviour_Valid_ShouldThrowWithErrors()
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
                    test2 = 2
                }, new CancellationToken(),
                Substitute.For<RequestHandlerDelegate<string>>()));
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
                test1 = "test",
                test2 = 6
            }, new CancellationToken(),
            Substitute.For<RequestHandlerDelegate<string>>());
    }
}