using MediatR;

namespace Microsoft.Extensions.DependencyInjection.Moodle.Commands;

public record MoodleLoginCommand : IRequest<string>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}