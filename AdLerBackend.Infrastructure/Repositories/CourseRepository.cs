using AdLerBackend.Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    public Task<CourseEntity> CreateCourse(CourseEntity course)
    {
        throw new NotImplementedException();
    }
}