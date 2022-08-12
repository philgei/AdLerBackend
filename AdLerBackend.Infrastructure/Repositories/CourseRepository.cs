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

    public async Task<CourseEntity> GetCourse(int id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var test = await db.Courses.Include(x => x.H5PFilesInCourse)
            .FirstOrDefaultAsync(x => x.Id == id);
        return test ?? throw new InvalidOperationException("Der Kurs wurde nicht gefunden");
    }

    public async Task<bool> ExistsCourseForUser(int authorId, string courseName)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var test = await db.Courses.AnyAsync(x => x.AuthorId == authorId && x.Name == courseName);
        return test;
    }
}