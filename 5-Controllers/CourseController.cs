using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Professor")]
    [HttpPost("/api/courses")]
    public async Task<IActionResult> CreateCourseAsync([FromBody] CourseDto courseDto)
    {
        // Fluent validation
        ValidationResult validationResult = _validator.Validate(courseDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map Dto to Course object
        Course course = _mapper.Map<Course>(courseDto);

        // Retreive created courseDto
        CourseDto createdCourse = await _courseService.CreateCourseAsync(course);

        return Created("/", createdCourse);
    }

    [HttpGet("/api/courses")]
    public async Task<IActionResult> GetAllCoursesAsync()
    {
        List<CourseDto> courses = await _courseService.GetAllCoursesAsync();

        return Ok(courses);
    }

    [HttpGet("/api/courses/{courseId}")]
    public async Task<IActionResult> GetCourseByIdAsync([FromRoute] Guid courseId)
    {
        CourseDto? course = await _courseService.GetCourseByIdAsync(courseId);

        if (course == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(course);
    }

    [Authorize(Roles = "Professor")]
    [HttpDelete("/api/courses/{courseId}")]
    public async Task<IActionResult> RemoveCourseAsync([FromRoute] Guid courseId)
    {
        bool result = await _courseService.RemoveCourseAsync(courseId);

        // If no lesson with this id exists
        if (!result)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return NoContent();
    }

    [Authorize(Roles = "Professor")]
    [HttpPut("/api/courses")]
    public async Task<IActionResult> UpdateCourseAsync([FromBody] CourseDto courseDto)
    {
        // Fluent validation on DTO:
        if (courseDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(courseDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map to Course object
        Course course = _mapper.Map<Course>(courseDto);

        CourseDto? result = await _courseService.UpdateCourseAsync(course);

        if (result == null)
            return BadRequest(new ResourceNotFoundError(courseDto.Id.ToString()));

        return NoContent();
    }



    public void Dispose()
    {
        _courseService.Dispose();
    }
}
