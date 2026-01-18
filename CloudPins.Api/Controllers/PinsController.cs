using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetAll;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/pins")]
public class PinsController : ControllerBase
{
    private readonly CreatePinCommandHandler _createHandler;
    private readonly GetPinByIdQueryHandler _getByIdHandler;
    private readonly GetPinsFeedQueryHandler _feedHandler;

    public PinsController(
        CreatePinCommandHandler createHandler,
        GetPinByIdQueryHandler getByIdHandler,
        GetPinsFeedQueryHandler feedHandler
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
            nameof(GetById),
            new { id = result.Id },
            result
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var pin = await _getByIdHandler.Handle(new GetPinByIdQuery(id), ct);
        if(pin is null) return NotFound();

        return Ok(pin);
    }

    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed(CancellationToken ct)
    {
        var currentUserId = Guid.NewGuid();
        var feed = await _feedHandler.Handle(
            new GetPinsFeedQuery(),
            currentUserId,
            ct
        );
        return Ok(feed);
    }
}