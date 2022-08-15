namespace AdLerBackend.Application.Common.Responses;

public class MoodleUserDataResponse
{
    public string MoodleUserName { get; set; }
    public bool IsAdmin { get; set; }
    public int UserId { get; set; }
}