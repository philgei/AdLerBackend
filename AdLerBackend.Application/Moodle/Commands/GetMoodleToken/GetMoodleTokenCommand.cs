using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Moodle.Commands.GetMoodleToken;

public record GetMoodleTokenCommand : IRequest<MoodleUserTokenDTO>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}