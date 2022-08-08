﻿using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Moodle;

public class MoodleWebApi : IMoodle
{
    private readonly HttpClient _client;


    public MoodleWebApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<MoodleUserTokenDTO> GetMoodleUserTokenAsync(string userName, string password)
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

        return new MoodleUserTokenDTO
        {
            moodleToken = resp.token
        };
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
        try
        {
            options ??= new PostToMoodleOptions();
            switch (options.Endpoint)
            {
                case PostToMoodleOptions.Endpoints.Webservice:
                    return await _client.PostAsync("https://moodle.cluuub.xyz/webservice/rest/server.php",
                        new FormUrlEncodedContent(wsParams));
                case PostToMoodleOptions.Endpoints.Login:
                    return await _client.PostAsync("https://moodle.cluuub.xyz/login/token.php",
                        new FormUrlEncodedContent(wsParams));
                default:
                    throw new Exception("Unbekannter Endpunkt");
            }
        }
        catch (Exception e)
        {
            throw new Exception("Die Moodle Web Api ist nicht erreichbar", e);
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