using AdLerBackend.Application.Common.Behaviours;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.GetMoodleToken;
using AdLerBackend.Application.Moodle.GetUserData;
using MediatR;
using MediatR.Pipeline;

namespace AdLerBackend.Application.UnitTests.Common.Behaviours;

public class ExceptionHandlingBehaviourTest
{
    [Test]
    public Task ExceptionBehaviour_ValidPath_ShouldCreateTokenException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserDataResponse>, MoodleUserDataResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidTokenException>(() =>
            systemUnderTest.Handle(new GetMoodleUserDataCommand(), new LmsException
                {
                    LmsErrorCode = "invalidtoken"
                },
                new RequestExceptionHandlerState<MoodleUserDataResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task ExceptionBehaviour_ValidPath_ShouldCreateLoginException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserTokenResponse>, MoodleUserTokenResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidMoodleLoginException>(() =>
            systemUnderTest.Handle(new GetMoodleTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidlogin"
                },
                new RequestExceptionHandlerState<MoodleUserTokenResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task ExceptionBehaviour_Invalid_ShourldReturnInputException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<MoodleUserTokenResponse>, MoodleUserTokenResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<LmsException>(() =>
            systemUnderTest.Handle(new GetMoodleTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidErrorCode"
                },
                new RequestExceptionHandlerState<MoodleUserTokenResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }
}