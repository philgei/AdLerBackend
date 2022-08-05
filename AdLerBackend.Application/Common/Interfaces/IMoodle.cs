namespace AdLerBackend.Application.Common.Interfaces;

public interface IMoodle
{
    /// <summary>
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns>The Moodle Token</returns>
    Task<MoodleUserDataDTO> GetMoodleTokenAsync(string userName, string password);
    Task<MoodleUserDataDTO> GetMoodleUserDataAsync(string token);
}


// TODO: DTOs should be in the Application layer.
public class MoodleUserDataDTO
{
    public string moodleToken { get; set; }
    public string moodleUserName { get; set; }
    public bool isAdmin { get; set; }
}

public class MoodleUserTokenDTO
{
    public string moodleToken { get; set; }
}