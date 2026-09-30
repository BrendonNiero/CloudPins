using CloudPins.Api.Common;
using CloudPins.Api.Models.DTO;
using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetAll;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using CloudPins.Application.Pins.LikePin;
using CloudPins.Application.Pins.UnlikePin;
using CloudPins.Infrastructure.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/pins")]
public class PinsController : ControllerBase
{
    private readonly CreatePinCommandHandler _createHandler;
    private readonly GetPinByIdQueryHandler _getByIdHandler;
    private readonly GetPinsFeedQueryHandler _feedHandler;
    private readonly GetFeedByPinQueryHandler _feedByPinHandler;
    private readonly GetSearchFeedQueryHandler _getSearchHandler;
    private readonly LikePinCommandHandler _likePinHandler;
    private readonly UnlikePinCommandHandler _unlikePinHandler;
    private readonly ElasticsearchService _elasticsearchService;

    public PinsController(
        CreatePinCommandHandler createHandler,
        GetPinByIdQueryHandler getByIdHandler,
        GetPinsFeedQueryHandler feedHandler,
        GetFeedByPinQueryHandler feedByPinHandler,
        GetSearchFeedQueryHandler getSearchHandler,
        LikePinCommandHandler likePinHandler,
        UnlikePinCommandHandler unlikePinHandler,
        ElasticsearchService elasticsearchService
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
        _feedHandler = feedHandler;
        _feedByPinHandler = feedByPinHandler;
        _getSearchHandler = getSearchHandler;
        _likePinHandler = likePinHandler;
        _unlikePinHandler = unlikePinHandler;
        _elasticsearchService = elasticsearchService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreatePinRequest request,
        CancellationToken ct
    )
    {
        var currentUserId = HttpContext.GetUserId();

        using var ms = new MemoryStream();
        await request.Image.CopyToAsync(ms, ct);

        var command = new CreatePinCommand
        {
          BoardId = request.BoardId,
          ImageBytes = ms.ToArray(),
          ImageFileName = request.Image.FileName,
          ImageContentType = request.Image.ContentType,
          Title = request.Title,
          Description = request.Description,
          TagIds = request.TagIds
        };

        var result = await _createHandler.Handle(command, currentUserId, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result
        );
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var currentUserId = HttpContext.GetUserId();
        var pin = await _getByIdHandler
            .Handle(new GetPinByIdQuery(id, currentUserId), ct);
        if(pin is null) return NotFound();

        return Ok(pin);
    }

    [Authorize]
    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed(CancellationToken ct)
    {
        var feed = await _feedHandler.Handle(
            new GetPinsFeedQuery(),
            ct
        );
        return Ok(feed);
    }

    [Authorize]
    [HttpGet("feed/{id:guid}")]
    public async Task<IActionResult> GetFeedByPin(Guid id, CancellationToken ct)
    {
        var feedByPin = await _feedByPinHandler.Handle(
            new FeedByPinQuery(id),
            ct
        );
        return Ok(feedByPin);
    }

    [Authorize]
    [HttpGet("/search/{search}")]
    public async Task<IActionResult> Search(string search, 
        [FromQuery] int  page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct= default)
    {
        if(string.IsNullOrWhiteSpace(search))
            return BadRequest("Search  term is required.");

        var results = await _elasticsearchService.SearchAsync(
            search,
            page,
            pageSize,
            ct
        );

        return Ok(results);
    }

    [Authorize]
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> LikePin(Guid id, CancellationToken ct)
    {
        var currentUserId = HttpContext.GetUserId();

        await _likePinHandler.Handle(id, currentUserId, ct);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:guid}/like")]
    public async Task<IActionResult> UnlikePin(Guid id, CancellationToken ct)
    {
        var currentUserId = HttpContext.GetUserId();

        await _unlikePinHandler.Handle(id, currentUserId, ct);

        return NoContent();
    }
}
