namespace Domain.Entities;

public class CourseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> H5PFilesInCourse { get; set; }
    public int AuthorId { get; set; }
}