using AdLerBackend.Application.Course.UploadCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.Courses;

[Microsoft.AspNetCore.Components.Route("api/Courses")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateCourse")]
    public async Task<bool> CreateCourse(IFormFile backupFile, IFormFile dslFile, [FromQuery] string token)
    {
        return await _mediator.Send(new UploadCourseCommand
        {
            H5PFileSteam = backupFile.OpenReadStream(),
            DslFileStream = dslFile.OpenReadStream(),
            WebServiceToken = token
        });
    }
}