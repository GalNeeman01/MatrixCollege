using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[ApiController]
public class LessonController : ControllerBase, IDisposable
{
    private LessonService _lessonService;
    private IValidator<LessonDto> _validator;
    private CourseService _courseService;
    private IMapper _mapper;

    public LessonController(LessonService lessonService, IValidator<LessonDto> validator, IMapper mapper, CourseService courseService)
    {
        _lessonService = lessonService;
        _validator = validator;
        _mapper = mapper;
        _courseService = courseService;
    }

    [Authorize(Roles = "Professor")]
    [HttpPost("/api/lessons")]
    public async Task<IActionResult> AddLessonAsync([FromBody] LessonDto lessonDto)
    {
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDto == null)
            return BadRequest(new RequestDataError());

        if (await _courseService.IsCourseExistsAsync(lessonDto.CourseId) == false)
            return BadRequest(new ResourceNotFoundError(lessonDto.CourseId.ToString()));

        ValidationResult validationResult = _validator.Validate(lessonDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map to Lesson
        Lesson lesson = _mapper.Map<Lesson>(lessonDto);

        // Call to service
        LessonDto dbLesson = await _lessonService.AddLessonAsync(lesson);

        return Created("/", dbLesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons")]
    public async Task<IActionResult> GetAllLessonsAsync()
    {
        List<LessonDto> lessons = await _lessonService.GetAllLessonsAsync();

        return Ok(lessons);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons/{lessonId}")]
    public async Task<IActionResult> GetLessonByIdAsync([FromRoute] Guid lessonId)
    {
        LessonDto? lesson = await _lessonService.GetLessonByIdAsync(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons-by-course/{courseId}")]
    public async Task<IActionResult> GetLessonsByCourseIdAsync([FromRoute] Guid courseId)
    {
        return Ok(await _lessonService.GetLessonsByCourseIdAsync(courseId));
    }

    [HttpGet("/api/lessons-info-by-course/{courseId}")]
    public async Task<IActionResult> GetLessonsInfoByCourseIdAsync([FromRoute] Guid courseId)
    {
        return Ok(await _lessonService.GetLessonsInfoByCourseIdAsync(courseId));
    }

    [Authorize(Roles = "Professor")]
    [HttpDelete("/api/lessons/{lessonId}")]
    public async Task<IActionResult> RemoveLessonAsync([FromRoute] Guid lessonId)
    {
        bool result = await _lessonService.RemoveLessonAsync(lessonId);

        // If no lesson with this id exists
        if (!result)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return NoContent();
    }

    [Authorize(Roles = "Professor")]
    [HttpPut("/api/lessons/{lessonId}")]
    public async Task<IActionResult> UpdateLessonAsync([FromRoute] Guid lessonId, [FromBody] LessonDto lessonDto)
    {
        // Fluent validation on DTO:
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(lessonDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map to Lesson object
        Lesson lesson = _mapper.Map<Lesson>(lessonDto);
        lesson.Id = lessonId;

        // Call to service
        LessonDto? resultLessonDto = await _lessonService.UpdateLessonAsync(lesson);

        if (resultLessonDto == null) return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(resultLessonDto);
    }

    public void Dispose()
    {
        _lessonService.Dispose();
    }
}
