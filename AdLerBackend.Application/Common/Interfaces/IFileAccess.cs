using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public List<string> StoreH5pFilesForCourse(CourseStoreDto courseToStore);
}