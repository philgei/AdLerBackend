using AdLerBackend.Application.Test;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : Controller
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET
    [HttpGet(Name = "GetTest")]
    public async Task<string> TestRoute()
    {
        return await _mediator.Send(new TestQuery());
    }
}