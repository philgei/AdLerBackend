using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAcess
{
    public Task<string> StoreCourse(CourseStoreDto courseToStore);
}