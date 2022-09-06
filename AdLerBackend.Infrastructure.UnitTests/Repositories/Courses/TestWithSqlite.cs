using AdLerBackend.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

public abstract class TestWithSqlite : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;

    protected readonly AdLerBackendDbContext DbContext;

    protected TestWithSqlite()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();
        var options = new DbContextOptionsBuilder<AdLerBackendDbContext>()
            .UseSqlite(_connection)
            .Options;
        DbContext = new AdLerBackendDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Close();
    }
}