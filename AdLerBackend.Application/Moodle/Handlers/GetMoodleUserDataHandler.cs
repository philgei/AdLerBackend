using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Moodle.Handlers;

public class GetMoodleUserDataHandler : IRequestHandler<GetMoodleUserDataCommand, MoodleUserDataDTO>
{
    private readonly IMoodle _moodleContext;

    public GetMoodleUserDataHandler(IMoodle moodleContext)
    {
        _moodleContext = moodleContext;
    }

    public async Task<MoodleUserDataDTO> Handle(GetMoodleUserDataCommand request, CancellationToken cancellationToken)
    {
        var moodleUserDataDto = await _moodleContext.GetMoodleUserDataAsync(request.WebServiceToken);
        return moodleUserDataDto;
    }
}