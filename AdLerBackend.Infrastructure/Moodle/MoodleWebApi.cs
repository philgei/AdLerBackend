using System.Net.Http.Json;
using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    // Sollte injected werden
    private static readonly HttpClient Client = new();

    public async Task<MoodleUserDataDTO> LogInUserAsync(string userName, string password)
    {
        HttpResponseMessage loginResponse;
        try
        {
            loginResponse = await Client.GetAsync(
                $"https://moodle.cluuub.xyz/login/token.php?username={userName}&password={password}&service=moodle_mobile_app");
        }
        catch (Exception e)
        {
            throw new Exception("Die Moodle Web Api ist nicht erreichbar", e);
        }

        var responseString = await loginResponse.Content.ReadAsStringAsync();
        UserTokenResponse loginResponseData;
        try
        {
            // Response cant be null, it will throw an exception if it is
            loginResponseData = JsonSerializer.Deserialize<UserTokenResponse>(responseString)!;
        }
        catch (Exception e)
        {
            throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
        }

        if (loginResponseData?.token == null)
        {
            ErrorResponse errorResponse;
            try
            {
                // Response cant be null, it will throw an exception if it is - PG
                errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseString)!;
            }
            catch (Exception e)
            {
                throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
            }

            if (errorResponse.errorcode == "invalidlogin")
                throw new InvalidMoodleLoginException("Invalid login credentials");
        }


        HttpResponseMessage userResponse;
        try
        {
            userResponse = await Client.PostAsync("https://moodle.cluuub.xyz/webservice/rest/server.php",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        {"wstoken", loginResponseData!.token},
                        {"wsfunction", "core_webservice_get_site_info"},
                        {"moodlewsrestformat", "json"}
                    }
                )
            );
        }
        catch (Exception e)
        {
            throw new Exception("Die Moodle Web Api ist nicht erreichbar", e);
        }


        UserDataResponse userDataResponse;
        try
        {
            // Response cant be null, it will throw an exception if it is - PG
            userDataResponse = (await userResponse.Content.ReadFromJsonAsync<UserDataResponse>())!;
        }
        catch (Exception e)
        {
            throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
        }

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

public class ErrorResponse
{
    public string error { get; set; }
    public string errorcode { get; set; }
    public object stacktrace { get; set; }
    public object debuginfo { get; set; }
    public object reproductionlink { get; set; }
}