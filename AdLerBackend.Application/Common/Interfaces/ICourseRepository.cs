using Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ICourseRepository
{
    Task<CourseEntity> CreateCourse(CourseEntity course);
    Task<CourseEntity> GetCourse(int id);
    Task<IList<CourseEntity>> GetAllCoursesForAuthor(int authorId);

    Task<bool> ExistsCourseForUser(int authorId, string courseName);
}