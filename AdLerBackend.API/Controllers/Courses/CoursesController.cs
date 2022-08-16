using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using AdLerBackend.Application.Course.GetCoursesForAuthor;
using AdLerBackend.Application.Course.GetCoursesForUser;
using AdLerBackend.Application.Course.GetLearningWorldDSL;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.Courses;

public class CoursesController : BaseApiController
{
    public CoursesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<bool> CreateCourse(IFormFile backupFile, IFormFile dslFile, [FromQuery] string token)
    {
        return await Mediator.Send(new UploadCourseCommand
        {
            BackupFileStream = backupFile.OpenReadStream(),
            DslFileStream = dslFile.OpenReadStream(),
            WebServiceToken = token
        });
    }

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

    [HttpGet]
    public async Task<ActionResult<GetCourseOverviewResponse>> GetCoursesForUser([FromHeader] string token)
    {
        return await Mediator.Send(new GetCoursesForUserCommand
        {
            WebServiceToken = token
        });
    }
}