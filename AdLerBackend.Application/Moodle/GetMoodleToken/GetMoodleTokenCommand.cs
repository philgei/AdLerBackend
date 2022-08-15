using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Moodle.GetMoodleToken;

public record GetMoodleTokenCommand : IRequest<MoodleUserTokenResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}