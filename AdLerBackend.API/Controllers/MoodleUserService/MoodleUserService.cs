using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.GetMoodleToken;
using AdLerBackend.Application.Moodle.GetUserData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers.MoodleUserService;

public class MoodleLoginController : BaseApiController
{
    public MoodleLoginController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("UserData")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<MoodleUserDataResponse>> GetMoodleUserData(
        [FromQuery] GetMoodleUserDataCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("Login")]
    public async Task<ActionResult<MoodleUserTokenResponse>> GetMoodleUserToken(
        [FromQuery] GetMoodleTokenCommand command)
    {
        return await Mediator.Send(command);
    }
}