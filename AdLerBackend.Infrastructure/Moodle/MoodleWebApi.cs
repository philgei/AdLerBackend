using System.Net.Http.Json;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    private static readonly HttpClient client = new();

    public async Task<MoodleUserDataDTO> LogInUserAsync(string userName, string password)
    {
        var loginResponse = await client.GetAsync(
            $"https://moodle.cluuub.xyz/login/token.php?username={userName}&password={password}&service=moodle_mobile_app");
        var loginResponseData = await loginResponse.Content.ReadFromJsonAsync<UserTokenResponse>();

        var userResponse = await client.PostAsync("https://moodle.cluuub.xyz/webservice/rest/server.php",
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"wstoken", loginResponseData.token},
                    {"wsfunction", "core_webservice_get_site_info"},
                    {"moodlewsrestformat", "json"}
                }
            )
        );

        var userDataResponse = await userResponse.Content.ReadFromJsonAsync<UserDataResponse>();

        return new MoodleUserDataDTO
        {
            moodleToken = loginResponseData.token,
            moodleUserName = userDataResponse.username,
            isAdmin = userDataResponse.userissiteadmin
        };
    }
}

public class UserTokenResponse
{
    public string token { get; set; }
}

public class UserDataResponse
{
    public string username { get; set; }
    public bool userissiteadmin { get; set; }
}