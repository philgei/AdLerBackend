using AdLerBackend.Application.Moodle.GetLearningWorldDSL;
using AdLerBackend.Controllers.LearningWorld;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.Test.Controllers.MoodleUserService;

public class LearningWorldController_test
{
    [Test]
    public async Task GetWorldDsl_Valid_CallsHandler()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LearningWorldController(mediatorMock);

        // Act
        await controller.GetWorldDsl(new GetLearningWorldDslCommand
        {
            CourseName = "TestName",
            WebServiceToken = "TestToken"
        });

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetLearningWorldDslCommand>());
    }
}