using CloudPins.Application.Tags.Create;
using CloudPins.Application.Tags.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/tags")]
public class TagsController : ControllerBase
{
    private readonly CreateTagCommandHandler _createHandler;
    private readonly GetAllTagsQueryHanddler _getAllHandler;

    public TagsController(CreateTagCommandHandler createHandler, GetAllTagsQueryHanddler getAllHandler)
    {
        _createHandler = createHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTagCommand command,
        CancellationToken ct
    )
    {
        var tag =  await _createHandler.Handle(command, ct);
        return Ok(tag);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var tags = await _getAllHandler.Handle(ct);
        return Ok(tags);
    }
}