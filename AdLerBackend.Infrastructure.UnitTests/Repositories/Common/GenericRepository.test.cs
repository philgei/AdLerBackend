using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.Common;
using AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Common;

public class GenericRepositoryTest : TestWithSqlite
{
    [TearDown]
    public void TearDown()
    {
        // empty the database
        DbContext.Courses.RemoveRange(DbContext.Courses);
    }

    [Test]
    public async Task Add_Valid_AddsAEntityToDB()
    {
        // Arrange
        var testEntity = new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 1,
            DslLocation = "Test Dsl Location"
        };
        var repository = new GenericRepository<CourseEntity>(DbContext);

        // Act
        await repository.AddAsync(testEntity);

        // Assert, that the entity was added to the database
        var entity = DbContext.Courses.FirstOrDefault();
        Assert.NotNull(entity);
    }

    [Test]
    public async Task Delete_Valid_DeletesEntityFromDb()
    {
        // Arrange
        var testEntity = new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 1,
            DslLocation = "Test Dsl Location"
        };
        var repository = new GenericRepository<CourseEntity>(DbContext);

        // Act
        await repository.AddAsync(testEntity);

        await repository.DeleteAsync(1);

        // Assert, that the entity was deleted from the database
        var entity = DbContext.Courses.FirstOrDefault();
        Assert.Null(entity);
    }

    [Test]
    public async Task Exists_Valid_ReturnsTrueIfEntitEsists()
    {
        // Arrange
        var testEntity = new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 1,
            DslLocation = "Test Dsl Location"
        };
        var repository = new GenericRepository<CourseEntity>(DbContext);

        // Act
        await repository.AddAsync(testEntity);

        var exists = await repository.Exists(1);

        // Assert
        Assert.True(exists);
    }

    [Test]
    public async Task Exists_Valid_ReturnsFalseIfEntitDoesNotEsists()
    {
        // Arrange

        var repository = new GenericRepository<CourseEntity>(DbContext);

        // Act

        var exists = await repository.Exists(1);

        // Assert
        Assert.False(exists);
    }

    [Test]
    public async Task GetAll_Valid_GetsAllEntites()
    {
        // Arrange
        var testEntity = new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 1,
            DslLocation = "Test Dsl Location"
        };
        var repository = new GenericRepository<CourseEntity>(DbContext);

        // Act
        await repository.AddAsync(testEntity);

        testEntity.Id = 2;
        await repository.AddAsync(testEntity);

        var allEntities = await repository.GetAllAsync();

        // Assert
        Assert.That(allEntities, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task Update_Valid_UpdatesEntity()
    {
        // Arrange
        var testEntity = new CourseEntity
        {
            Id = 1,
            Name = "Test Course",
            AuthorId = 1,
            DslLocation = "Test Dsl Location"
        };
        var repository = new GenericRepository<CourseEntity>(DbContext);

        await repository.AddAsync(testEntity);

        // Act
        testEntity.Name = "Updated Name";
        await repository.UpdateAsync(testEntity);

        // Assert
        var entity = DbContext.Courses.FirstOrDefault();
        Assert.That(entity.Name, Is.EqualTo("Updated Name"));
    }
}