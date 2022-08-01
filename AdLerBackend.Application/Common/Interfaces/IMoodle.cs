namespace AdLerBackend.Application.Common.Interfaces;

public interface IMoodle
{
    Task<MoodleUserDataDTO> LogInUserAsync(string userName, string password);
}

public class MoodleUserDataDTO
{
    public string moodleToken { get; set; }
    public string moodleUserName { get; set; }
    public bool isAdmin { get; set; }
}