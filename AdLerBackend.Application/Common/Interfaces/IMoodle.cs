using AdLerBackend.Application.Common.Responses;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IMoodle
{
    /// <summary>
    ///     Gets the Moodle User Data for a given Webserice Token
    /// </summary>
    /// <param name="token">Moodle Webservice Token</param>
    /// <returns>Moodle User Data6</returns>
    Task<MoodleUserDataResponse> GetMoodleUserDataAsync(string token);

    /// <summary>
    ///     Gets the Moodle Webservice Token for a given Account
    /// </summary>
    /// <param name="userName">Moodle User Name</param>
    /// <param name="password">Moodle User Password</param>
    /// <returns>The Moodle Token</returns>
    Task<MoodleUserTokenResponse> GetMoodleUserTokenAsync(string userName, string password);

    /// <summary>
    ///     Searches all Courses, that are avalibale for the given Moodle User
    /// </summary>
    /// <param name="token">Token of the Moodle User</param>
    /// <param name="searchString">The Course to get Searched for</param>
    /// <returns>A List of all found Coruses</returns>
    Task<MoodleSearchCoursesResponse> SearchCoursesAsync(string token, string searchString);

    /// <summary>
    ///     Gets the Contents of a Course
    /// </summary>
    /// <param name="token">Token of the Moodle User</param>
    /// <param name="courseId">ID of the Course</param>
    /// <returns>All User-Visible Contents of a Course as Array</returns>
    Task<CourseContent[]> GetCourseContentAsync(string token, int courseId);
}