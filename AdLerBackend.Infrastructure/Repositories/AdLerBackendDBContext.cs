using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdLerBackendDbContext : DbContext
{
    public AdLerBackendDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CourseEntity> Courses { get; set; }
    //public DbSet<H5PLocationEntity> H5PLocations { get; set; }
}