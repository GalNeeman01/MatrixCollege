using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class ProgressController : ControllerBase, IDisposable
{
    private ProgressService _progressService;
    private ProgressValidator _validator;

    public ProgressController(ProgressService progressService, ProgressValidator validatior)
    {
        _progressService = progressService;
        _validator = validatior;
    }

    [HttpGet("/api/progress/{userId}")]
    public IActionResult GetUserProgress([FromRoute] Guid userId)
    {
        return Ok(_progressService.GetUserProgress(userId));
    }

    [HttpPost("/api/progress")]
    public IActionResult AddProgress([FromBody] Progress progress)
    {
        if (progress == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(progress);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        Progress dbProgress = _progressService.AddProgress(progress);

        return Created("/", dbProgress);
    }

    public void Dispose()
    {
        _progressService.Dispose();
    }
}
