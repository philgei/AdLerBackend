using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.Course.UploadCourse;

public record UploadCourseCommand : CommandWithToken<bool>
{
    public Stream Content { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
}