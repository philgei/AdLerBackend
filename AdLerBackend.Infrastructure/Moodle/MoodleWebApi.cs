using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    // TODO: Sollte injected werden
    private readonly HttpClient _client;


    public MoodleWebApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<MoodleUserTokenDTO> GetMoodleUserTokenAsync(string userName, string password)
    {
        HttpResponseMessage loginResponse;
        try
        {
            loginResponse = await _client.GetAsync(
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

        if (loginResponseData?.token != null)
            return new MoodleUserTokenDTO
            {
                moodleToken = loginResponseData.token
            };
        {
            MoodleTokenErrorResponse moodleTokenErrorResponse;
            try
            {
                // Response cant be null, it will throw an exception if it is - PG
                moodleTokenErrorResponse = JsonSerializer.Deserialize<MoodleTokenErrorResponse>(responseString)!;
            }
            catch (Exception e)
            {
                throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
            }

            if (moodleTokenErrorResponse.errorcode == "invalidlogin")
                throw new InvalidMoodleLoginException("Invalid login credentials");
        }

        throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden");
    }


    public async Task<MoodleUserDataDTO> GetMoodleUserDataAsync(string token)
    {
        var resp = await MoodleCallAsync<UserDataResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"wsfunction", "core_webservice_get_site_info"},
            {"moodlewsrestformat", "json"}
        });

        return new MoodleUserDataDTO
        {
            moodleUserName = resp.username,
            isAdmin = resp.userissiteadmin
        };
    }

    private async Task<TDtoType> MoodleCallAsync<TDtoType>(Dictionary<string, string> wsParams)
    {
        var moodleApiResponse = await PostToMoodleAsync(wsParams);
        var responseString = await moodleApiResponse.Content.ReadAsStringAsync();

        return ParseResponseFromString<TDtoType>(responseString);
    }


    private TResponseType ParseResponseFromString<TResponseType>(string responseString)
    {
        ThrowIfMoodleError(responseString);

        return TryRead<TResponseType>(responseString);
    }

    private static TResponse TryRead<TResponse>(string responseString)
    {
        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseString)!;
        }
        catch (Exception e)
        {
            throw new Exception("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
        }
    }

    private void ThrowIfMoodleError(string responseString)
    {
        var errorData = TryRead<MoodleWSErrorResponse>(responseString);
        if (errorData.exception != null)
            throw new LmsException
            {
                LmsMessage = errorData.message,
                LmsErrorCode = errorData.errorcode
            };
    }

    private async Task<HttpResponseMessage> PostToMoodleAsync(Dictionary<string, string> wsParams)
    {
        HttpResponseMessage response;
        try
        {
            response =
                await _client.PostAsync("https://moodle.cluuub.xyz/webservice/rest/server.php",
                    new FormUrlEncodedContent(wsParams));
        }
        catch (Exception e)
        {
            throw new Exception("Die Moodle Web Api ist nicht erreichbar", e);
        }

        return response;
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

public class MoodleTokenErrorResponse
{
    public string error { get; set; }
    public string errorcode { get; set; }
    public object stacktrace { get; set; }
    public object debuginfo { get; set; }
    public object reproductionlink { get; set; }
}

public class MoodleWSErrorResponse
{
    public string exception { get; set; }
    public string errorcode { get; set; }
    public string message { get; set; }
}