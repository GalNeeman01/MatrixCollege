using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase, IDisposable
{
    private CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost("/api/courses")]
    public IActionResult CreateCourse([FromBody] Course course)
    {
        return Created("/", _courseService.CreateCourse(course));
    }

    public void Dispose()
    {
        _courseService.Dispose();
    }
}
