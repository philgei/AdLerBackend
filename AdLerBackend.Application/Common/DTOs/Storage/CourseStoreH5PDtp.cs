namespace AdLerBackend.Application.Common.DTOs.Storage;

public class CourseStoreH5PDto : CourseBaseStorageDto
{
    public IList<H5PDto> H5PFiles { get; set; }
}