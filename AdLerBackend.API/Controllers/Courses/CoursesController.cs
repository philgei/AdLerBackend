using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Course.CourseManagement.DeleteCourse;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using AdLerBackend.Application.Course.CourseManagement.UploadH5pBase;
using AdLerBackend.Application.Course.GetCourseDetail;
using AdLerBackend.Application.Course.GetCoursesForAuthor;
using AdLerBackend.Application.Course.GetCoursesForUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

#pragma warning disable CS1573

namespace AdLerBackend.API.Controllers.Courses;

/// <summary>
///     Manages all the Courses
/// </summary>
public class CoursesController : BaseApiController
{
    public CoursesController(IMediator mediator) : base(mediator)
    {
    }


    /// <summary>
    ///     Gets all Courses that a Author has created
    /// </summary>
    /// <param name="token"></param>
    /// <param name="authorId"></param>
    /// <returns></returns>
    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<GetCourseOverviewResponse>> GetCoursesForAuthor([FromHeader] string token,
        int authorId)
    {
        return Ok(await Mediator.Send(new GetCoursesForAuthorCommand
        {
            AuthorId = authorId,
            WebServiceToken = token
        }));
    }

    /// <summary>
    ///     Gets the World File of a Course
    /// </summary>
    /// <returns></returns>
    [HttpGet("{courseId}")]
    public async Task<ActionResult<LearningWorldDtoResponse>> GetWorldDsl([FromHeader] string token,
        [FromRoute] int courseId)

    {
        return await Mediator.Send(new GetCourseDetailCommand
        {
            CourseId = courseId,
            WebServiceToken = token
        });
    }

    /// <summary>
    ///     Gets All Courses a User is enrolled in
    /// </summary>
    /// <param name="token">The Users WebService Token</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<GetCourseOverviewResponse>> GetCoursesForUser([FromHeader] string token)
    {
        return await Mediator.Send(new GetCoursesForUserCommand
        {
            WebServiceToken = token
        });
    }

    /// <summary>
    ///     Uploads a Course to the Backend
    ///     Beware: The Course also has to be imported into moodle manually
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CreateCourse(IFormFile backupFile, IFormFile dslFile, [FromHeader] string token)
    {
        return await Mediator.Send(new UploadCourseCommand
        {
            BackupFileStream = backupFile.OpenReadStream(),
            DslFileStream = dslFile.OpenReadStream(),
            WebServiceToken = token
        });
    }


    /// <summary>
    ///     Uploads a base h5p file needed to render h5ps in the Frontend
    /// </summary>
    /// <param name="h5PBaseFile"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("H5PBase")]
    public async Task<bool> UploadBaseH5P(IFormFile h5PBaseFile, [FromHeader] string token)
    {
        return await Mediator.Send(new UploadH5PBaseCommand
        {
            WebServiceToken = token,
            H5PBaseZipStream = h5PBaseFile.OpenReadStream()
        });
    }

    /// <summary>
    ///     Deletes a Course by Id
    /// </summary>
    /// <param name="token"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [HttpDelete("{courseId}")]
    public async Task<bool> DeleteCourse([FromHeader] string token, [FromRoute] int courseId)
    {
        return await Mediator.Send(new DeleteCourseCommand
        {
            CourseId = courseId,
            WebServiceToken = token
        });
    }
}