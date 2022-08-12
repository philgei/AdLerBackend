namespace AdLerBackend.Application.Common.DTOs;

public class CourseStoreH5pDto : CourseBaseStorageDto
{
    public IList<H5PDto> H5PFiles { get; set; }
}