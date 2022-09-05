using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Course.CourseManagement.UploadH5pBase;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Course.CourseManagement.UploadH5PBase;

public class UploadH5PBaseTest
{
    private IFileAccess _fileAccess;
    private IMoodle _moodle;
    private UploadH5PBaseHandler _systemUnderTest;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _fileAccess = Substitute.For<IFileAccess>();
        _systemUnderTest = new UploadH5PBaseHandler(_moodle, _fileAccess);
    }

    [Test]
    public void Handle_UserNotAuthorized_Throws()
    {
        // Arrange
        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
            {
                IsAdmin = false,
                UserId = 1,
                MoodleUserName = "MoodleUser"
            }
        );

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            _systemUnderTest.Handle(new UploadH5PBaseCommand(), CancellationToken.None));
    }

    [Test]
    public void Handle_Valud_ShouldCallFileStorage()
    {
        // Arrange
        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
            {
                IsAdmin = true,
                UserId = 1,
                MoodleUserName = "MoodleUser"
            }
        );

        // Act
        _systemUnderTest.Handle(new UploadH5PBaseCommand(), CancellationToken.None);

        // Assert
        _fileAccess.Received(1).StoreH5PBase(Arg.Any<Stream>());
    }
}