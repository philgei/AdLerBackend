﻿using AdLerBackend.Application.Course.CourseManagement.GetCoursesForAuthor;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
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
            BackupFileStream = backupFile.OpenReadStream(),
            DslFileStream = dslFile.OpenReadStream(),
            WebServiceToken = token
        });
    }

    [HttpGet("GetCoursesForAuthor")]
    public async Task<IActionResult> GetCoursesForAuthor([FromQuery] GetCoursesForAuthorCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}