using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Moodle.Commands;

namespace AdLerBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoodleLoginController
{
    private readonly IMediator _mediator;

    public MoodleLoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] APIMoodleLoginRequestDTO request)
    {
        return await _mediator.Send(new MoodleLoginCommand
        {
            Password = request.Password,
            UserName = request.Username
        });
    }
}