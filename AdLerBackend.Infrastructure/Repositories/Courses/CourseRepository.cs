using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.Courses;

public class CourseRepository : GenericRepository<CourseEntity>, ICourseRepository
{
    public CourseRepository(BaseAdLerBackendDbContext dbContext) : base(dbContext)
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

    public new async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public new async Task<CourseEntity?> GetAsync(int id)
    {
        //return Task.FromResult(Context.Courses.Where(c => c.Id == id).Include(c => c.H5PFilesInCourse).First());
        // include h5pLocations in the query


        // Get the course and include the h5pLocations
        var course = await Context.Courses.Where(c => c.Id == id).Include(c => c.H5PFilesInCourse)
            .FirstOrDefaultAsync();
        return course;
    }
}