using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.Course.CourseManagement.DeleteCourse;

public record DeleteCourseCommand : CommandWithToken<bool>
{
    public int CourseId { get; init; }
}