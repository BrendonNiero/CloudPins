using CloudPins.Api.Common;
using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetAll;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/pins")]
public class PinsController : ControllerBase
{
    private readonly CreatePinCommandHandler _createHandler;
    private readonly GetPinByIdQueryHandler _getByIdHandler;
    private readonly GetPinsFeedQueryHandler _feedHandler;
    private readonly GetFeedByPinQueryHandler _feedByPinHandler;

    public PinsController(
        CreatePinCommandHandler createHandler,
        GetPinByIdQueryHandler getByIdHandler,
        GetPinsFeedQueryHandler feedHandler,
        GetFeedByPinQueryHandler feedByPinHandler
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
        _feedHandler = feedHandler;
        _feedByPinHandler = feedByPinHandler;
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
        var feed = await _feedHandler.Handle(
            new GetPinsFeedQuery(),
            ct
        );
        return Ok(feed);
    }

    [HttpGet("feed/{id:guid}")]
    public async Task<IActionResult> GetFeedByPin(Guid id, CancellationToken ct)
    {
        var feedByPin = await _feedByPinHandler.Handle(
            new FeedByPinQuery(id),
            ct
        );
        return Ok(feedByPin);
    }
}