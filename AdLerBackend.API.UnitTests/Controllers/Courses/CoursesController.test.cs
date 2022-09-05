using AdLerBackend.API.Controllers.Courses;
using AdLerBackend.Application.Course.CourseManagement.DeleteCourse;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using AdLerBackend.Application.Course.CourseManagement.UploadH5pBase;
using AdLerBackend.Application.Course.GetCourseDetail;
using AdLerBackend.Application.Course.GetCoursesForAuthor;
using AdLerBackend.Application.Course.GetCoursesForUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Courses;

public class CoursesControllerTest
{
    [Test]
    public async Task GetCoursesForAuthor_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);

        // Act
        await controller.GetCoursesForAuthor("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetCoursesForAuthorCommand>(x => x.WebServiceToken == "token" && x.AuthorId == 1337));
    }

    [Test]
    public async Task GetCoursesForUser_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);

        // Act
        await controller.GetCoursesForUser("token");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetCoursesForUserCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task CreateCourse_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);
        var backupFile = Substitute.For<IFormFile>();
        var dslFile = Substitute.For<IFormFile>();

        // Act
        await controller.CreateCourse(backupFile, dslFile, "token");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<UploadCourseCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task UploadBaseH5p_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);
        var baseH5PFile = Substitute.For<IFormFile>();

        // Act
        await controller.UploadBaseH5P(baseH5PFile, "token");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<UploadH5PBaseCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task DeleteCourse_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);

        // Act
        await controller.DeleteCourse("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<DeleteCourseCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task GetWorldDsl_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new CoursesController(mediatorMock);

        // Act
        await controller.GetWorldDsl("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetCourseDetailCommand>(x => x.WebServiceToken == "token"));
    }
}