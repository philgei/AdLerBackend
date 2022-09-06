using AdLerBackend.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

public class CourseRepositoryTest : TestWithSqlite
{
    private SqliteConnection _connection;
    private DbContextOptions<AdLerBackendDbContext> _options;


    [Test]
    public async Task Repository_CanConnectToDB()
    {
        Assert.That(await DbContext.Database.CanConnectAsync(), Is.True);
    }
}