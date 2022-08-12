using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdLerBackendDbContext : DbContext
{
    public AdLerBackendDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CourseEntity> Courses { get; set; }
    private DbSet<H5PLocationEntity> H5PLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseEntity>()
            .HasMany(x => x.H5PFilesInCourse);
    }
}