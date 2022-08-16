using Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ICourseRepository : IGenericRepository<CourseEntity>
{
    Task<IList<CourseEntity>> GetAllCoursesForAuthor(int authorId);

    Task<bool> ExistsCourseForAuthor(int authorId, string courseName);

    Task<IList<CourseEntity>> GetAllCoursesByStrings(List<string> searchStrings);
}