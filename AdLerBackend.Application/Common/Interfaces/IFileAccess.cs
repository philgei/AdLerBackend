using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public List<string>? StoreH5PFilesForCourse(CourseStoreH5pDto courseToStoreH5P);
    public string StoreDSLFileForCourse(StoreCourseDslDto courseToStoreH5P);
}