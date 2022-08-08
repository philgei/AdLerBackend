using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetMoodleToken;
using AdLerBackend.Application.Moodle.Handlers;
using NSubstitute;

namespace Application.UnitTests.Moodle.Handlers;

public class GerMoodleUserTokenHandler_test
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
        _moodleMock.GetMoodleUserTokenAsync(request.UserName, request.Password).Returns(new MoodleUserTokenDTO
        {
            moodleToken = "token"
        });

        // Act
        var result = await _systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        await _moodleMock.Received(1).GetMoodleUserTokenAsync(request.UserName, request.Password);
        Assert.That("token", Is.EqualTo(result.moodleToken));
    }
}