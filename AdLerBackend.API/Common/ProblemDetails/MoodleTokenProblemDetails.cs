using System.Net;

namespace AdLerBackend.Common.ProblemDetails;

public class MoodleTokenProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public MoodleTokenProblemDetails()
    {
        Title = "The Moodle token Provided is invalid";
        Status = (int) HttpStatusCode.Unauthorized;
        Type = "https://httpstatuses.com/401";
    }
}