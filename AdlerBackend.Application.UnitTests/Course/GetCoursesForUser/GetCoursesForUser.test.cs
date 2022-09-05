using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Course.GetCoursesForUser;
using AdLerBackend.Domain.Entities;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Course.GetCoursesForUser;

public class GetCoursesForUserTest
{
    private ICourseRepository _courseRepository;
    private IMoodle _moodle;


    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _courseRepository = Substitute.For<ICourseRepository>();
    }

    [Test]
    public async Task Handle_Valid_RetunsCoursesForUser()
    {
        // Arrange

        var systemUnderTest = new GetCoursesForUserHandler(_moodle, _courseRepository);

        var request = new GetCoursesForUserCommand
        {
            WebServiceToken = "testToken"
        };

        _moodle.GetCoursesForUserAsync(Arg.Any<string>()).Returns(new MoodleCourseListResponse
        {
            Total = 1,
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1,
                    Fullname = "FullName"
                }
            }
        });

        _courseRepository.GetAllCoursesByStrings(Arg.Any<List<string>>()).Returns(new List<CourseEntity>
        {
            new()
            {
                Id = 1,
                Name = "FullName"
            }
        });

        // Act
        var result = await systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Courses.Count, Is.EqualTo(1));
    }

    [Test]
    public Task Handle_Invalid_CoursesInDbDontMatchWithMoodle_ThrowsException()
    {
        // Arrange

        var systemUnderTest = new GetCoursesForUserHandler(_moodle, _courseRepository);

        var request = new GetCoursesForUserCommand
        {
            WebServiceToken = "testToken"
        };

        _moodle.GetCoursesForUserAsync(Arg.Any<string>()).Returns(new MoodleCourseListResponse
        {
            Total = 0,
            Courses = new List<MoodleCourse>()
        });

        _courseRepository.GetAllCoursesByStrings(Arg.Any<List<string>>()).Returns(new List<CourseEntity>
        {
            new()
            {
                Id = 1,
                Name = "FullName"
            }
        });

        // Act
        var result =
            Assert.ThrowsAsync<Exception>(async () => await systemUnderTest.Handle(request, CancellationToken.None));

        // Assert
        Assert.That(result?.Message, Contains.Substring("the number"));
        return Task.CompletedTask;
    }
}