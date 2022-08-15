using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetCourse;

public record GetCourseCommand : CommandWithToken<GetCourseOverviewResponse>
{
    public int CourseId { get; set; }
}