using AdLerBackend.Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly IDbContextFactory<AdLerBackendDbContext> _dbContextFactory;

    public CourseRepository(IDbContextFactory<AdLerBackendDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<CourseEntity> CreateCourse(CourseEntity course)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        var test = await db.Courses.AddAsync(course);

        await db.SaveChangesAsync();

        return course;
    }
}