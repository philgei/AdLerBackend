using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using MediatR;

namespace AdLerBackend.Application.Moodle.Commands.GetMoodleToken;

public record GetMoodleTokenCommand : IRequest<MoodleUserTokenResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}