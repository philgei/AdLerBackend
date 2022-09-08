using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Repositories;

public sealed class ProductionContext : BaseAdLerBackendDbContext
{
    private readonly IConfiguration _configuration;

    public ProductionContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Get password from environment variable
        var password = _configuration["ASPNETCORE_DBPASSWORD"];
        options.UseMySql(_configuration.GetConnectionString("ProductionConnection") + ";password=" + password,
            new MariaDbServerVersion(new Version(10, 9, 2)));
    }
}