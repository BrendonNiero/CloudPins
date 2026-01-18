using CloudPins.Application.Boards.Create;
using CloudPins.Application.Boards.GetById;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/boards")]
public class BoardsController : ControllerBase
{
    private readonly CreateBoardCommandHandler _createHandler;
    private readonly GetBoardByIdQueryHandler _getByIdHandler;

    public BoardsController(
        CreateBoardCommandHandler createHandler,
        GetBoardByIdQueryHandler getByIdHandler
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBoardCommand command,
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
        var board = await _getByIdHandler.Handle(new GetBoardByIdQuery(id), ct);

        if(board is null) return NotFound();

        return Ok(board);
    }
}