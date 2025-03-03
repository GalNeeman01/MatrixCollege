using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[ApiController]
public class CourseController : ControllerBase, IDisposable
{
    private CourseService _courseService;
    private IValidator<CourseDto> _validator;
    private IMapper _mapper;

    public CourseController(CourseService courseService, IValidator<CourseDto> validator, IMapper mapper)
    {
        _courseService = courseService;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpPost("/api/courses")]
    public IActionResult CreateCourse([FromBody] CourseDto courseDto)
    {
        // Fluent validation
        ValidationResult validationResult = _validator.Validate(courseDto);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        // Map Dto to Course object
        Course course = _mapper.Map<Course>(courseDto);

        // Retreive created courseDto
        CourseDto createdCourse = _courseService.CreateCourse(course);

        return Created("/", createdCourse);
    }

    [HttpGet("/api/courses")]
    public IActionResult GetAllCourses()
    {
        List<CourseDto> courses = _courseService.GetAllCourses();

        return Ok(courses);
    }

    [HttpGet("/api/courses/{courseId}")]
    public IActionResult GetCourseById([FromRoute] Guid courseId)
    {
        CourseDto? course = _courseService.GetCourseById(courseId);

        if (course == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(course);
    }

    [HttpDelete("/api/courses/{courseId}")]
    public IActionResult RemoveCourse([FromRoute] Guid courseId)
    {
        bool result = _courseService.RemoveCourse(courseId);

        // If no lesson with this id exists
        if (!result)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return NoContent();
    }

    public void Dispose()
    {
        _courseService.Dispose();
    }
}
