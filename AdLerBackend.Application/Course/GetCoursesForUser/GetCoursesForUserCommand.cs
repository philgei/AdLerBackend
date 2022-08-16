using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetCoursesForUser;

public record GetCoursesForUserCommand : CommandWithToken<GetCourseOverviewResponse>
{
}