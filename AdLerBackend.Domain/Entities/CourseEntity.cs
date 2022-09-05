using AdLerBackend.Domain.Entities.Common;

namespace AdLerBackend.Domain.Entities;

public class CourseEntity : BaseEntity
{
    public string Name { get; set; }

    public List<H5PLocationEntity> H5PFilesInCourse { get; set; }
    public string DslLocation { get; set; }
    public int AuthorId { get; set; }
}