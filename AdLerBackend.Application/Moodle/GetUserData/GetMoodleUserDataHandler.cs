using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Moodle.GetUserData;

public class GetMoodleUserDataHandler : IRequestHandler<GetMoodleUserDataCommand, MoodleUserDataResponse>
{
    private readonly IMoodle _moodleContext;

    public GetMoodleUserDataHandler(IMoodle moodleContext)
    {
        _moodleContext = moodleContext;
    }

    public async Task<MoodleUserDataResponse> Handle(GetMoodleUserDataCommand request, CancellationToken cancellationToken)
    {
        var moodleUserDataDto = await _moodleContext.GetMoodleUserDataAsync(request.WebServiceToken);
        return moodleUserDataDto;
    }
}