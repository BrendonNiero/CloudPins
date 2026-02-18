using CloudPins.Application.Users.Get;
using Microsoft.AspNetCore.Mvc;

namespace CloudPins.Api.Controllers;

[ApiController]
[Route("/user")]
public class ProFileController : ControllerBase
{
    private readonly GetProfileQueryHandler _getHandler;

    public ProFileController()
    {
        
    }
}