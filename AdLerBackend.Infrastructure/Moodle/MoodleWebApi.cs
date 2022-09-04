using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

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
            MoodleToken = resp.Token
        };
    }

    public async Task<CourseContent[]> GetCourseContentAsync(string token, int courseId)
    {
        var resp = await MoodleCallAsync<CourseContent[]>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_get_contents"},
            {"courseid", courseId.ToString()}
        });
        return resp;
    }

    public async Task<MoodleCourseListResponse> GetCoursesForUserAsync(string token)
    {
        return await MoodleCallAsync<MoodleCourseListResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_search_courses"},
            {"criterianame", "search"},
            {"criteriavalue", ""},
            {"limittoenrolled", "1"}
        });
    }

    public async Task<bool> IsMoodleAdminAsync(string token)
    {
        var userData = await GetMoodleUserDataAsync(token);
        return userData.IsAdmin;
    }


    public virtual async Task<MoodleUserDataResponse> GetMoodleUserDataAsync(string token)
    {
        var resp = await MoodleCallAsync<UserDataResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"wsfunction", "core_webservice_get_site_info"},
            {"moodlewsrestformat", "json"}
        });

        return new MoodleUserDataResponse
        {
            MoodleUserName = resp.Username,
            IsAdmin = resp.Userissiteadmin,
            UserId = resp.Userid
        };
    }

    public async Task<MoodleCourseListResponse> SearchCoursesAsync(string token, string searchString,
        bool limitToEnrolled = false)
    {
        var resp = await MoodleCallAsync<MoodleCourseListResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_search_courses"},
            {"criterianame", "search"},
            {"criteriavalue", searchString},
            {"limittoenrolled", limitToEnrolled ? "1" : "0"}
        });

        return resp;
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
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(responseString, options)!;
        }
        catch (Exception e)
        {
            throw new LmsException("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
        }
    }

    private void ThrowIfMoodleError(string responseString)
    {
        MoodleWsErrorResponse wsErrorData = null!;
        try
        {
            wsErrorData = TryRead<MoodleWsErrorResponse>(responseString);
        }
        catch (Exception e)
        {
            // ignored
        }

        if (wsErrorData?.Errorcode != null)
            throw new LmsException
            {
                LmsErrorCode = wsErrorData.Errorcode
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
                    url = "https://testmoodle.cluuub.xyz/webservice/rest/server.php";
                    break;
                case PostToMoodleOptions.Endpoints.Login:
                    url = "https://testmoodle.cluuub.xyz/login/token.php";
                    break;
            }

            return await _client.PostAsync(url,
                new FormUrlEncodedContent(wsParams));
        }
        catch (Exception e)
        {
            throw new LmsException("Die Moodle Web Api ist nicht erreichbar: URL: " + url, e);
        }
    }
}

public class UserTokenResponse
{
    public string Token { get; set; }
}

public class UserDataResponse
{
    public string Username { get; set; }
    public bool Userissiteadmin { get; set; }
    public int Userid { get; set; }
}

public class MoodleWsErrorResponse
{
    public string Errorcode { get; set; }
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