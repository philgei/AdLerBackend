namespace AdLerBackend.Application.Common.DTOs.Storage;

public class StoreCourseDslDto : CourseBaseStorageDto
{
    public Stream DslFile { get; set; }
}