using AdLerBackend.Application.Course.CourseManagement.GetCoursesForAuthor;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.Courses;

public class CoursesController : BaseApiController
{
    public CoursesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("CreateCourse")]
    public async Task<bool> CreateCourse(IFormFile backupFile, IFormFile dslFile, [FromQuery] string token)
    {
        return await Mediator.Send(new UploadCourseCommand
        {
            BackupFileStream = backupFile.OpenReadStream(),
            DslFileStream = dslFile.OpenReadStream(),
            WebServiceToken = token
        });
    }

    [HttpGet("GetCoursesForAuthor")]
    public async Task<IActionResult> GetCoursesForAuthor([FromQuery] GetCoursesForAuthorCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}