using CloudPins.Api.Common;
using CloudPins.Api.Models.DTO;
using CloudPins.Application.Users.Get;
using CloudPins.Application.Users.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/user")]
public class ProFileController : ControllerBase
{
    private readonly GetProfileQueryHandler _getHandler;
    private readonly UpdateProfileCommandHandler _updateHandler;

    public ProFileController(
        GetProfileQueryHandler getHandler,
        UpdateProfileCommandHandler updateHandler
    )
    {
        _getHandler = getHandler;
        _updateHandler = updateHandler;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var currentUserId = HttpContext.GetUserId();
        var profile = await _getHandler.Handle(currentUserId, ct);
        return Ok(profile);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromForm] UpdateProfileRequest request,
        CancellationToken ct
    )
    {
        var currentUserId = HttpContext.GetUserId();

        using var ms = new MemoryStream();
        await request.Image.CopyToAsync(ms, ct);

        var command = new UpdateProfileCommand
        {
            Name = request.Name,
            ImageBytes = ms.ToArray(),
            ImageFileName = request.Image.FileName,
            ImageContentType = request.Image.ContentType
        };

        var result = await _updateHandler.Handle(command, currentUserId, ct);

        return Ok(result);
    }
}