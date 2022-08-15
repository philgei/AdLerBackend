using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetLearningWorldDSL;

public record GetLearningWorldDslCommand : CommandWithToken<LearningWorldDtoResponse>
{
    public string CourseName { get; init; }
}