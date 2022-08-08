using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    private readonly HttpClient _client;


    public MoodleWebApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<MoodleUserTokenResponse> GetMoodleUserTokenAsync(string userName, string password)
    {
        var resp = await MoodleCallAsync<UserTokenResponse>(new Dictionary<string, string>
        {
            {"username", userName},
            {"password", password},
            {"service", "moodle_mobile_app"}
        }, new PostToMoodleOptions
        {
            Endpoint = PostToMoodleOptions.Endpoints.Login
        });

        return new MoodleUserTokenResponse
        {
            moodleToken = resp.token
        };
    }


    public async Task<MoodleUserDataResponse> GetMoodleUserDataAsync(string token)
    {
        var resp = await MoodleCallAsync<UserDataResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"wsfunction", "core_webservice_get_site_info"},
            {"moodlewsrestformat", "json"}
        });

        return new MoodleUserDataResponse
        {
            moodleUserName = resp.username,
            isAdmin = resp.userissiteadmin
        };
    }

    private async Task<TDtoType> MoodleCallAsync<TDtoType>(Dictionary<string, string> wsParams,
        PostToMoodleOptions? options = null)
    {
        var moodleApiResponse = await PostToMoodleAsync(wsParams, options);
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
        var wsErrorData = TryRead<MoodleWSErrorResponse>(responseString);
        if (wsErrorData.errorcode != null)
            throw new LmsException
            {
                LmsErrorCode = wsErrorData.errorcode
            };
    }

    private async Task<HttpResponseMessage> PostToMoodleAsync(Dictionary<string, string> wsParams,
        PostToMoodleOptions? options = null)
    {
        var url = "";

        try
        {
            options ??= new PostToMoodleOptions();
            switch (options.Endpoint)
            {
                case PostToMoodleOptions.Endpoints.Webservice:
                    url = "https://moodle.cluuub.xyz/webservice/rest/server.php";
                    break;
                case PostToMoodleOptions.Endpoints.Login:
                    url = "https://moodle.cluuub.xyz/login/token.php";
                    break;
            }

            return await _client.PostAsync(url,
                new FormUrlEncodedContent(wsParams));
        }
        catch (Exception e)
        {
            throw new Exception("Die Moodle Web Api ist nicht erreichbar: URL: " + url, e);
        }
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

public class MoodleWSErrorResponse
{
    public string errorcode { get; set; }
}

public class PostToMoodleOptions
{
    public enum Endpoints
    {
        Webservice,
        Login
    }

    public Endpoints Endpoint { get; set; } = Endpoints.Webservice;
}