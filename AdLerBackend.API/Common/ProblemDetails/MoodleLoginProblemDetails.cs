using System.Net;

namespace AdLerBackend.API.Common.ProblemDetails;

public class MoodleLoginProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public MoodleLoginProblemDetails()
    {
        Title = "Moodle login error";
        Status = (int) HttpStatusCode.Unauthorized;
        Type = "https://httpstatuses.com/401";
    }
}