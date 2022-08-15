using Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ICourseRepository : IGenericRepository<CourseEntity>
{
    Task<IList<CourseEntity>> GetAllCoursesForAuthor(int authorId);

    Task<bool> ExistsCourseForUser(int authorId, string courseName);
}