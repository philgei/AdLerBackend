using Domain.Entities.Common;

namespace Domain.Entities;

public class CourseEntity : BaseEntity
{
    public string Name { get; set; }

    //public List<H5PLocationEntity> H5PFilesInCourse { get; set; }
    public int AuthorId { get; set; }
}