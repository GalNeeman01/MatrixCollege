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

    [HttpGet("/api/courses")]
    public IActionResult GetAllCourses()
    {
        List<Course> courses = _courseService.GetAllCourses();

        return Ok(courses);
    }

    [HttpGet("/api/courses/{courseId}")]
    public IActionResult GetCourseById([FromRoute] Guid courseId)
    {
        Course? course = _courseService.GetCourseById(courseId);

        if (course == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(course);
    }

    public void Dispose()
    {
        _courseService.Dispose();
    }
}
