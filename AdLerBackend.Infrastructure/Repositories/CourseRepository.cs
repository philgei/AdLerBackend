using AdLerBackend.Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository : GenericRepository<CourseEntity>, ICourseRepository
{
    public CourseRepository(AdLerBackendDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IList<CourseEntity>> GetAllCoursesForAuthor(int authorId)
    {
        var allCoursesForAuthor = await Context.Courses.Where(x => x.AuthorId == authorId).ToListAsync();

        return allCoursesForAuthor;
    }

    public async Task<bool> ExistsCourseForAuthor(int authorId, string courseName)
    {
        var test = await Context.Courses.AnyAsync(x => x.AuthorId == authorId && x.Name == courseName);
        return test;
    }


    public async Task<IList<CourseEntity>> GetAllCoursesByStrings(List<string> searchStrings)
    {
        return await Context.Courses.Where(c => searchStrings.Contains(c.Name)).ToListAsync();
    }

    public async Task<CourseEntity> GetAsync(int? id)
    {
        if (id is null) return null;

        // include h5pLocations in the query
        return Context.Courses.Where(c => c.Id == id).Include(c => c.H5PFilesInCourse).First();
    }

    public new async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }
}