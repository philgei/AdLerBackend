using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Course.CourseManagement.GetCourse;

public record GetCourseCommand : CommandWithToken<GetCoursesResponse>
{
    public int CourseId { get; set; }
}