using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class ProgressController : ControllerBase, IDisposable
{
    private ProgressService _progressService;

    public ProgressController(ProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet("/api/progress/{userId}")]
    public IActionResult GetUserProgress([FromRoute] Guid userId)
    {
        return Ok(_progressService.GetUserProgress(userId));
    }

    public void Dispose()
    {
        _progressService.Dispose();
    }
}
