using CloudPins.Application.Users.Create;
using CloudPins.Application.Users.Login;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
    private readonly CreateUserCommandHandler _createHandler;
    private readonly LoginCommandHandler _loginHandler;

    public AuthController(
        CreateUserCommandHandler createHandler,
        LoginCommandHandler loginHandler
    )
    {
        _createHandler = createHandler;
        _loginHandler = loginHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserCommand command,
        CancellationToken ct
    )
    {
        var result = await _createHandler.Handle(command, ct);
        return Created("", result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct
    )
    {
        var result = await _loginHandler.Handle(command, ct);
        return Ok(result);
    }
}