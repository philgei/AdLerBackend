using AdLerBackend.Application.Common.DTOs.Storage;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public List<string>? StoreH5PFilesForCourse(CourseStoreH5PDto courseToStoreH5P);
    public string StoreDslFileForCourse(StoreCourseDslDto courseToStoreH5P);
    public Stream GetFileStream(string filePath);
    public string StoreH5PBase(Stream fileStream);
    public bool DeleteCourse(CourseDeleteDto courseToDelete);
}