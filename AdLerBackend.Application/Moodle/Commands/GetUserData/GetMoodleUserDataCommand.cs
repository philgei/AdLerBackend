using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using MediatR;

namespace AdLerBackend.Application.Moodle.Commands.GetUserData;

public record GetMoodleUserDataCommand : IRequest<MoodleUserDataResponse>
{
    public string WebServiceToken { get; init; }
}