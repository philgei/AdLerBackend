using MediatR;
using Microsoft.Extensions.DependencyInjection.Moodle.Commands;

namespace Microsoft.Extensions.DependencyInjection.Moodle.Handlers;

public class LogUserIntoMoodleHandler : IRequestHandler<MoodleLoginCommand, string>
{
    public Task<string> Handle(MoodleLoginCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult("[FAKE] " + request.UserName + " Logged in to Moodle");
    }
}