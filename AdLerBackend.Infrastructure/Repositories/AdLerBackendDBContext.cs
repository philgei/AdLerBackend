using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdLerBackendDbContext : DbContext
{
    public AdLerBackendDbContext(DbContextOptions options) : base(options)
    {
        Courses.Add(new CourseEntity
        {
            Id = 1337
        });
    }

    public DbSet<CourseEntity> Courses { get; set; }
}