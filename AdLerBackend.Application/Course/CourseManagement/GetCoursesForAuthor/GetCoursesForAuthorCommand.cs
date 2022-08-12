using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Course.CourseManagement.GetCoursesForAuthor;

public record GetCoursesForAuthorCommand : CommandWithToken<GetCoursesResponse>
{
    public int AuthorId { get; init; }
}