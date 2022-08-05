using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Moodle.Commands.GetMoodleToken;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers.MoodleLogin;

[ApiController]
[Route("api/[controller]")]
public class MoodleLoginController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoodleLoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GetUserData")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<MoodleUserDataDTO>> GetMoodleUserData([FromBody] GetMoodleUserDataCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("GetUserToken")]
    public async Task<ActionResult<MoodleUserTokenDTO>> GetMoodleUserToken([FromBody] GetMoodleTokenCommand command)
    {
        return await _mediator.Send(command);
    }
}