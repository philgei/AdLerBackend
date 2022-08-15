using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Moodle.GetMoodleToken;
using AdLerBackend.Application.Moodle.GetUserData;
using AdLerBackend.Controllers.MoodleUserService;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AdLerBackend.API.Test.Controllers.MoodleUserService;

public class MoodleLoginControllerTest
{
    [Test]
    public async Task GetUserData_ShouldForwardCallToMoodleLoginService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new MoodleLoginController(mediatorMock);

        // Act
        await controller.GetMoodleUserData(new GetMoodleUserDataCommand
        {
            WebServiceToken = "TestToken"
        });

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetMoodleUserDataCommand>(x => x.WebServiceToken == "TestToken"));
    }

    [Test]
    public Task GetUserData_ReturnsBadRequest_WhenLoginFails()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        mediatorMock.Send(Arg.Any<GetMoodleUserDataCommand>()).Throws(
            new InvalidMoodleLoginException());


        var controller = new MoodleLoginController(mediatorMock);

        // Expect exception to be thrown
        Assert.ThrowsAsync<InvalidMoodleLoginException>(() => controller.GetMoodleUserData(new GetMoodleUserDataCommand
        {
            WebServiceToken = "TestToken"
        }));
        return Task.CompletedTask;
    }

    [Test]
    public async Task Login_ShouldForwardCallToMoodleService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new MoodleLoginController(mediatorMock);

        // Act
        await controller.GetMoodleUserToken(new GetMoodleTokenCommand
        {
            Password = "TestPassword",
            UserName = "TestUsername"
        });

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetMoodleTokenCommand>(x => x.Password == "TestPassword" && x.UserName == "TestUsername"));
    }
}