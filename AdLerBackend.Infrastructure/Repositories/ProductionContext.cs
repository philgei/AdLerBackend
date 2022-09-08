using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Repositories;

public class ProductionContext : BaseAdLerBackendDbContext
{
    private readonly IConfiguration _configuration;

    public ProductionContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(_configuration.GetConnectionString("ProductionConnection"),
            new MariaDbServerVersion(new Version(10, 9, 2)));
    }
}