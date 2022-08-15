using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetLearningWorldDSL;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.LearningWorld;

public class LearningWorldController : BaseApiController
{
    public LearningWorldController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("WorldDSL")]
    public async Task<ActionResult<LearningWorldDtoResponse>> GetWorldDsl(
        [FromQuery] GetLearningWorldDslCommand command)
    {
        return await Mediator.Send(command);
    }
}