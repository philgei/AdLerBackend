using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetLearningWorldDSL;

public record GetCourseDetailCommand : CommandWithToken<LearningWorldDtoResponse>
{
    public int CourseId { get; init; }
}