using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

public abstract class TestWithSqlite : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;

    protected readonly BaseAdLerBackendDbContext DbContext;

    protected TestWithSqlite()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();
        var options = new DbContextOptionsBuilder<BaseAdLerBackendDbContext>()
            .UseSqlite(_connection)
            .Options;
        DbContext = new BaseAdLerBackendDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Close();
    }

    [TearDown]
    public void TearDown()
    {
        // empty the database
        DbContext.Courses.RemoveRange(DbContext.Courses);
    }
}