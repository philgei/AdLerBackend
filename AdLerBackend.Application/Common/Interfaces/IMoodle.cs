namespace AdLerBackend.Application.Common.Interfaces;

public interface IMoodle
{
    public MoodleTokenDTO GetToken(string username, string password);
}

public class MoodleTokenDTO
{
}