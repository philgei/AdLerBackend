namespace AdLerBackend.Application.Common.Responses;

public class MoodleUserDataResponse
{
    public string moodleUserName { get; set; }
    public bool isAdmin { get; set; }
    public int userId { get; set; }
}