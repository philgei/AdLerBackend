namespace AdLerBackend.Application.Common.DTOs.Storage;

public class CourseStoreH5pDto : CourseBaseStorageDto
{
    public IList<H5PDto> H5PFiles { get; set; }
}