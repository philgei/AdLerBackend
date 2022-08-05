using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Moodle.Commands.GetUserData;

public record GetMoodleUserDataCommand : IRequest<MoodleUserDataDTO>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}