using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.GetMoodleToken;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Moodle.GetMoodleToken;

public class GerMoodleUserTokenHandlerTest
{
    private IMoodle _moodleMock;
    private GetMoodleUserTokenHandler _systemUnderTest;

    [SetUp]
    public void SetUp()
    {
        _moodleMock = Substitute.For<IMoodle>();
        _systemUnderTest = new GetMoodleUserTokenHandler(_moodleMock);
    }

    [Test]
    public async Task Handle_Should_Return_Token()
    {
        // Arrange
        var request = new GetMoodleTokenCommand
        {
            UserName = "username",
            Password = "password"
        };
        _moodleMock.GetMoodleUserTokenAsync(request.UserName, request.Password).Returns(new MoodleUserTokenResponse
        {
            MoodleToken = "token"
        });

        // Act
        var result = await _systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        await _moodleMock.Received(1).GetMoodleUserTokenAsync(request.UserName, request.Password);
        Assert.That("token", Is.EqualTo(result.MoodleToken));
    }
}