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
    private IMapper _mapper;

    public LessonController(LessonService lessonService, IValidator<LessonDto> validator, IMapper mapper)
    {
        _lessonService = lessonService;
        _validator = validator;
        _mapper = mapper;
    }

    [Authorize(Roles = "Professor")]
    [HttpPost("/api/lessons")]
    public async Task<IActionResult> AddLessonAsync([FromBody] LessonDto lessonDto)
    {
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(lessonDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map to Lesson
        Lesson lesson = _mapper.Map<Lesson>(lessonDto);

        // Call to service
        LessonDto dbLesson = await _lessonService.AddLesson(lesson);

        return Created("/", dbLesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons")]
    public async Task<IActionResult> GetAllLessonsAsync()
    {
        List<LessonDto> lessons = await _lessonService.GetAllLessons();

        return Ok(lessons);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons/{lessonId}")]
    public async Task<IActionResult> GetLessonByIdAsync([FromRoute] Guid lessonId)
    {
        LessonDto? lesson = await _lessonService.GetLessonById(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("/api/lessons-by-course/{courseId}")]
    public async Task<IActionResult> GetLessonsByCourseIdAsync([FromRoute] Guid courseId)
    {
        return Ok(await _lessonService.GetLessonsByCourseId(courseId));
    }

    [Authorize(Roles = "Professor")]
    [HttpDelete("/api/lessons/{lessonId}")]
    public async Task<IActionResult> RemoveLessonAsync([FromRoute] Guid lessonId)
    {
        bool result = await _lessonService.RemoveLesson(lessonId);

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
        LessonDto? resultLessonDto = await _lessonService.UpdateLesson(lesson);

        if (resultLessonDto == null) return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(resultLessonDto);
    }

    public void Dispose()
    {
        _lessonService.Dispose();
    }
}
