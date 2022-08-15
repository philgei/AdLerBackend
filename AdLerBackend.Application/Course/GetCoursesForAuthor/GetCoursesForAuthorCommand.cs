using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetCoursesForAuthor;

public record GetCoursesForAuthorCommand : CommandWithToken<GetCourseOverviewResponse>
{
    public int AuthorId { get; init; }
}