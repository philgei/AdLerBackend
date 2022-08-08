using AdLerBackend.Application.Common.Behaviours;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetMoodleToken;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;
using MediatR.Pipeline;

namespace Application.UnitTests.Common.Behaviours;

public class ExceptionHandlingBehaviour_test
{
    [Test]
    public async Task ExceptionBehaviour_ValidPath_ShouldCreateTokenException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserDataDTO>, MoodleUserDataDTO, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidTokenException>(() =>
            systemUnderTest.Handle(new GetMoodleUserDataCommand(), new LmsException
                {
                    LmsErrorCode = "invalidtoken"
                },
                new RequestExceptionHandlerState<MoodleUserDataDTO>(), CancellationToken.None));
    }

    [Test]
    public async Task ExceptionBehaviour_ValidPath_ShouldCreateLoginException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserTokenDTO>, MoodleUserTokenDTO, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidMoodleLoginException>(() =>
            systemUnderTest.Handle(new GetMoodleTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidlogin"
                },
                new RequestExceptionHandlerState<MoodleUserTokenDTO>(), CancellationToken.None));
    }

    [Test]
    public async Task ExceptionBehaviour_Invalid_ShourldReturnInputException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserTokenDTO>, MoodleUserTokenDTO, LmsException>();

        // Act
        Assert.ThrowsAsync<LmsException>(() =>
            systemUnderTest.Handle(new GetMoodleTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidErrorCode"
                },
                new RequestExceptionHandlerState<MoodleUserTokenDTO>(), CancellationToken.None));
    }
}