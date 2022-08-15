using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Course.GetCourse;

public record GetCourseCommand : CommandWithToken<GetCoursesResponse>
{
    public int CourseId { get; set; }
}