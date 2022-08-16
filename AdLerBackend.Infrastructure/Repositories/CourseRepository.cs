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
}