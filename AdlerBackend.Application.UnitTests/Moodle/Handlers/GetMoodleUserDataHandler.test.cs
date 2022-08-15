﻿using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Moodle.GetUserData;
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
        _moodleMock.GetMoodleUserDataAsync(request.WebServiceToken).Returns(new MoodleUserDataResponse
        {
            IsAdmin = true,
            MoodleUserName = "TestNutzer"
        });
        // Act
        var result = await _systemUnderTest.Handle(request, new CancellationToken());

        // Assert
        await _moodleMock.Received(1).GetMoodleUserDataAsync(request.WebServiceToken);
        Assert.That(result.IsAdmin, Is.True);
        Assert.That(result.MoodleUserName, Is.EqualTo("TestNutzer"));
    }
}