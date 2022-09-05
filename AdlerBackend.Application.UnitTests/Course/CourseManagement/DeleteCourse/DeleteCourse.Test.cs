using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Course.CourseManagement.DeleteCourse;
using AdLerBackend.Domain.Entities;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Course.CourseManagement.DeleteCourse;

public class DeleteCourseTest
{
    private ICourseRepository _courseRepository;
    private IFileAccess _fileAccess;
    private IMoodle _moodle;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _courseRepository = Substitute.For<ICourseRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
    }

    [Test]
    public async Task Handle_Valid_ShouldCallDeletionOfCourse()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_moodle, _courseRepository, _fileAccess);

        _moodle.IsMoodleAdminAsync(Arg.Any<string>()).Returns(true);

        var courseMock = new CourseEntity
        {
            Id = 1
        };

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseMock);

        _fileAccess.DeleteCourse(Arg.Any<CourseDeleteDto>()).Returns(true);

        // Act
        var result = await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        // Expect DeleteCourse to be called once
        _fileAccess.Received(1).DeleteCourse(Arg.Any<CourseDeleteDto>());

        // Assert that DeleteAsync was called
        await _courseRepository.Received(1).DeleteAsync(Arg.Any<int>());
    }

    [Test]
    public Task Handle_UserNotAdmin_ShouldThorwException()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_moodle, _courseRepository, _fileAccess);
        _moodle.IsMoodleAdminAsync(Arg.Any<string>()).Returns(false);

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(async () => await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None));
        return Task.CompletedTask;
    }
}