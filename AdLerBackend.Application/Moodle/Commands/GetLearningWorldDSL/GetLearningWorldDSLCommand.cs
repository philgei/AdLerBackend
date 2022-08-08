using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Moodle.Commands.GetLearningWorldDSL;

public record GetLearningWorldDslCommand : CommandWithToken<LearningWorldDtoResponse>
{
    public string CourseName { get; init; }
}