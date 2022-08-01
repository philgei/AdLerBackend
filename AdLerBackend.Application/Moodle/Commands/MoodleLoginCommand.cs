using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection.Moodle.Commands;

public record MoodleLoginCommand : IRequest<MoodleUserDataDTO>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}