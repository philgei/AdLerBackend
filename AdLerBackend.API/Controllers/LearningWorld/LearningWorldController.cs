using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetLearningWorldDSL;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.LearningWorld;

[ApiController]
[Route("api/LearningWorld")]
public class LearningWorldController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearningWorldController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("WorldDSL")]
    public async Task<ActionResult<LearningWorldDtoResponse>> GetWorldDsl(
        [FromQuery] GetLearningWorldDslCommand command)
    {
        return await _mediator.Send(command);
    }
}