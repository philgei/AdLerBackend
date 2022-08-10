using Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ICourseRepository
{
    Task<CourseEntity> CreateCourse(CourseEntity course);
}