using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    public Task<MoodleUserDataDTO> LogInUserAsync(string userName, string password)
    {
        return Task.FromResult(new MoodleUserDataDTO()
        {
            isAdmin = true,
            moodleToken = "fakeToken",
            moodleUserName = userName,
        });
    }
}