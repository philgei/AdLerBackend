using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using AdLerBackend.Application.Course.GetCoursesForAuthor;
using AdLerBackend.Application.Course.GetLearningWorldDSL;
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

    [HttpGet("GetLearningWorldDSL")]
    public async Task<ActionResult<LearningWorldDtoResponse>> GetWorldDsl(
        [FromQuery] GetCourseDetailCommand command)
    {
        return await Mediator.Send(command);
    }
}