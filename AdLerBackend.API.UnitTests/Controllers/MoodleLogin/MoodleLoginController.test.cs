using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Moodle.Commands;
using AdLerBackend.Controllers.MoodleLogin;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AdLerBackend.API.Test.Controllers.MoodleLogin;

public class MoodleLoginControllerTest
{
    [Test]
    public async Task Login_ShouldForwardCallToMoodleLoginService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new MoodleLoginController(mediatorMock);

        // Act
        var result = await controller.Login(new MoodleLoginCommand
        {
            Password = "test123",
            UserName = "test123"
        });

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<MoodleLoginCommand>(x => x.Password == "test123" && x.UserName == "test123"));
    }

    [Test(Description = "Login returns bad request, when login fails")]
    public async Task Login_ReturnsBadRequest_WhenLoginFails()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        mediatorMock.Send(Arg.Any<MoodleLoginCommand>()).Throws(
            new InvalidMoodleLoginException());


        var controller = new MoodleLoginController(mediatorMock);

        // Expect exception to be thrown
        Assert.ThrowsAsync<InvalidMoodleLoginException>(() => controller.Login(new MoodleLoginCommand
        {
            Password = "test123",
            UserName = "test123"
        }));
    }
}