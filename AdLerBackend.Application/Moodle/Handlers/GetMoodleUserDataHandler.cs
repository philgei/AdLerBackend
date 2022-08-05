using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Moodle.Handlers;

public class LogUserIntoMoodleHandler : IRequestHandler<GetMoodleUserDataCommand, MoodleUserDataDTO>
{
    private readonly IMoodle _moodleContext;

    public LogUserIntoMoodleHandler(IMoodle moodleContext)
    {
        _moodleContext = moodleContext;
    }

    public async Task<MoodleUserDataDTO> Handle(GetMoodleUserDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var moodleUserDataDto = await _moodleContext.GetMoodleTokenAsync(request.UserName, request.Password);
            return moodleUserDataDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}