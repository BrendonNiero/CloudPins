using CloudPins.Api.Common;
using CloudPins.Application.Boards.Create;
using CloudPins.Application.Boards.GetAll;
using CloudPins.Application.Boards.GetById;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/boards")]
public class BoardsController : ControllerBase
{
    private readonly CreateBoardCommandHandler _createHandler;
    private readonly GetBoardByIdQueryHandler _getByIdHandler;
    private readonly GetAllBoardsQueryHandler _getAllHandler;

    public BoardsController(
        CreateBoardCommandHandler createHandler,
        GetBoardByIdQueryHandler getByIdHandler,
        GetAllBoardsQueryHandler getAllHandler
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getByIdHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBoardCommand command,
        CancellationToken ct
    )
    {
        var currentUserId = HttpContext.GetUserId();
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

        return Ok(board);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var currentUserId = HttpContext.GetUserId();
        var boards = await _getAllHandler.Handle(new GetAllBoardsQuery(currentUserId), ct);
        return Ok(boards);
    }
}