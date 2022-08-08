using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetMoodleToken;
using MediatR;

namespace AdLerBackend.Application.Moodle.Handlers;

public class GetMoodleUserTokenHandler : IRequestHandler<GetMoodleTokenCommand, MoodleUserTokenDTO>
{
    private readonly IMoodle _moodle;

    public GetMoodleUserTokenHandler(IMoodle moodle)
    {
        _moodle = moodle;
    }

    public async Task<MoodleUserTokenDTO> Handle(GetMoodleTokenCommand request, CancellationToken cancellationToken)
    {
        var moodleTokenDto = await _moodle.GetMoodleUserTokenAsync(request.UserName, request.Password);
        return moodleTokenDto;
    }
}