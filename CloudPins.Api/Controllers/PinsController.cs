using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetAll;
using CloudPins.Application.Pins.GetById;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("api/pins")]
public class PinsController : ControllerBase
{
    private readonly CreatePinCommandHandler _createHandler;
    private readonly GetPinByIdQueryHandler _getByIdHandler;
    private readonly GetPinsFeedQuery _feedHandler;

    public PinsController(
        CreatePinCommandHandler createHandler,
        GetPinByIdQueryHandler getByIdHandler,
        GetPinsFeedQuery feedHandler
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
        _feedHandler = feedHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePinCommand command,
        CancellationToken ct
    )
    {
        var currentUserId = Guid.NewGuid();

        var result = await _createHandler.Handle(command, currentUserId, ct);

        return CreatedAtAction(
            nameof(GetPinByIdQuery),
            new { id = result.Id },
            result
        );
    }

    [HttpGet("{id:guid}")]
    public 
}