using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using AdLerBackend.Application.Moodle.Handlers;
using NSubstitute;

namespace Application.UnitTests.Moodle.Handlers;

public class GetMoodleUserDataHandler_test
{
    private IMoodle _moodleMock;
    private GetMoodleUserDataHandler _systemUnderTest;

    [SetUp]
    public void SetUp()
    {
        _moodleMock = Substitute.For<IMoodle>();
        _systemUnderTest = new GetMoodleUserDataHandler(_moodleMock);
    }

    [Test]
    public async Task Handle_ValidResponse_CallsServiceAndReturns()
    {
        // Arrange
        var request = new GetMoodleUserDataCommand
        {
            WebServiceToken = "testToken"
        };
        _moodleMock.GetMoodleUserDataAsync(request.WebServiceToken).Returns(new MoodleUserDataDTO
        {
            isAdmin = true,
            moodleUserName = "TestNutzer"
        });
        // Act
        var result = await _systemUnderTest.Handle(request, new CancellationToken());

        // Assert
        await _moodleMock.Received(1).GetMoodleUserDataAsync(request.WebServiceToken);
        Assert.That(result.isAdmin, Is.True);
        Assert.That(result.moodleUserName, Is.EqualTo("TestNutzer"));
    }
}