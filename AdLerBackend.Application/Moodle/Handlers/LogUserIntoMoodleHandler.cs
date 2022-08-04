using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection.Moodle.Handlers;

public class LogUserIntoMoodleHandler : IRequestHandler<MoodleLoginCommand, MoodleUserDataDTO>
{
    private readonly IMoodle _moodleContext;

    public LogUserIntoMoodleHandler(IMoodle moodleContext)
    {
        _moodleContext = moodleContext;
    }

    public async Task<MoodleUserDataDTO> Handle(MoodleLoginCommand request, CancellationToken cancellationToken)
    {
        //return Task.FromResult("[FAKE] " + request.UserName + " Logged in to Moodle");
        return await _moodleContext.LogInUserAsync(request.UserName, request.Password);
    }
}