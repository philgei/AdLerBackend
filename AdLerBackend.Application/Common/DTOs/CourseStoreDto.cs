namespace AdLerBackend.Application.Common.DTOs;

public class CourseStoreDto
{
    public DslFileDto CourseInforamtion { get; set; }
    public IList<H5PDto> H5PFiles { get; set; }
    public int AuthorId { get; set; }
}