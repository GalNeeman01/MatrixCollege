using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase, IDisposable
{
    private CourseService _courseService;
    private IValidator<Course> _validator;

    public CourseController(CourseService courseService, IValidator<Course> validator)
    {
        _courseService = courseService;
        _validator = validator;
    }

    [HttpPost("/api/courses")]
    public IActionResult CreateCourse([FromBody] Course course)
    {
        // Fluent validation
        ValidationResult validationResult = _validator.Validate(course);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

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
