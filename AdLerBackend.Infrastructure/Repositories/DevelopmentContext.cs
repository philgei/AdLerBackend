using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Repositories;

public class DevelopmentContext : BaseAdLerBackendDbContext
{
    private readonly IConfiguration _configuration;

    public DevelopmentContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_configuration.GetConnectionString("DevelopomentConnection"));
    }
}