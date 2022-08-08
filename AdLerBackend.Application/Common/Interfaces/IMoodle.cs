using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IMoodle
{
    /// <summary>
    /// Gets the Moodle User Data for a given Webserice Token
    /// </summary>
    /// <param name="token">Moodle Webservice Token</param>
    /// <returns>Moodle User Data6</returns>
    Task<MoodleUserDataResponse> GetMoodleUserDataAsync(string token);

    /// <summary>
    /// Gets the Moodle Webservice Token for a given Account
    /// </summary>
    /// <param name="userName">Moodle User Name</param>
    /// <param name="password">Moodle User Password</param>
    /// <returns>The Moodle Token</returns>
    Task<MoodleUserTokenResponse> GetMoodleUserTokenAsync(string userName, string password);
}