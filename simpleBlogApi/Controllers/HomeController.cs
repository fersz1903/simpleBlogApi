using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using simpleBlogApi;

namespace simpleBlogApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("hello")]
    public IActionResult SayHello()
    {
        return Ok("Hello");
    }
}
