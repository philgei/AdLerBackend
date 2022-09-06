using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.Courses;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

public class CourseRepositoryTest : TestWithSqlite
{
    [Test]
    public async Task GetAllCoursesForAuthor_Valid_GetsThen()
    {
        // Arrange
        var systemUnderTest = new CourseRepository(DbContext);
        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 2,
            DslLocation = "Test Dsl Location"
        });

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 2,
            Name = "Test Course 2",
            AuthorId = 2,
            DslLocation = "Test Dsl Location 2"
        });

        // Act
        var result = await systemUnderTest.GetAllCoursesForAuthor(2);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ExistsCourseForAuthor_Valid_GetsResult()
    {
        // Arrange
        var systemUnderTest = new CourseRepository(DbContext);

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 2,
            DslLocation = "Test Dsl Location"
        });

        // Act
        var result = await systemUnderTest.ExistsCourseForAuthor(2, "Test Course");

        // Assert
        Assert.That(result, Is.True);

        // Act
        var result2 = await systemUnderTest.ExistsCourseForAuthor(2, "Test CourseFOO");

        // Assert
        Assert.That(result2, Is.False);
    }

    [Test]
    public async Task GetAllCoursesByStrings_Valid_GetsResult()
    {
        // Arrange
        var systemUnderTest = new CourseRepository(DbContext);

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 2,
            DslLocation = "Test Dsl Location"
        });

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 2,
            Name = "Test Course 2",
            AuthorId = 2,
            DslLocation = "Test Dsl Location 2"
        });

        // Act
        var result = await systemUnderTest.GetAllCoursesByStrings(new List<string> {"Test Course", "Test Course 2"});

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAsync_Valid_GetsResult()
    {
        // Arrange
        var systemUnderTest = new CourseRepository(DbContext);

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 2,
            DslLocation = "Test Dsl Location"
        });

        // Act
        var result = await systemUnderTest.GetAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Delete_Valid_DeletesCourse_DeletesCourse()
    {
        // Arrange
        var systemUnderTest = new CourseRepository(DbContext);

        await systemUnderTest.AddAsync(new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 2,
            DslLocation = "Test Dsl Location"
        });

        // Act
        await systemUnderTest.DeleteAsync(1);

        // Assert
        var allCourses = await systemUnderTest.GetAllAsync();
        Assert.That(allCourses, Has.Count.EqualTo(0));
    }
}