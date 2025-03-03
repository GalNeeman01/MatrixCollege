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

    [Authorize]
    [HttpPost("/api/lessons")]
    public IActionResult AddLesson([FromBody] LessonDto lessonDto)
    {
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(lessonDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        // Map to Lesson
        Lesson lesson = _mapper.Map<Lesson>(lessonDto);

        // Call to service
        LessonDto dbLesson = _lessonService.AddLesson(lesson);

        return Created("/", dbLesson);
    }

    [Authorize]
    [HttpGet("/api/lessons")]
    public IActionResult GetAllLessons()
    {
        List<LessonDto> lessons = _lessonService.GetAllLessons();

        return Ok(lessons);
    }

    [Authorize]
    [HttpGet("/api/lessons/{lessonId}")]
    public IActionResult GetLessonById([FromRoute] Guid lessonId)
    {
        LessonDto? lesson = _lessonService.GetLessonById(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }

    [Authorize]
    [HttpGet("/api/lessons-by-course/{courseId}")]
    public IActionResult GetLessonsByCourseId([FromRoute] Guid courseId)
    {
        return Ok(_lessonService.GetLessonsByCourseId(courseId));
    }

    [Authorize]
    [HttpDelete("/api/lessons/{lessonId}")]
    public IActionResult RemoveLesson([FromRoute] Guid lessonId)
    {
        bool result = _lessonService.RemoveLesson(lessonId);

        // If no lesson with this id exists
        if (!result)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return NoContent();
    }

    [Authorize]
    [HttpPut("/api/lessons/{lessonId}")]
    public IActionResult UpdateLesson([FromRoute] Guid lessonId, [FromBody] LessonDto lessonDto)
    {
        // Fluent validation on DTO:
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(lessonDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        // Map to Lesson object
        Lesson lesson = _mapper.Map<Lesson>(lessonDto);
        lesson.Id = lessonId;

        // Call to service
        LessonDto? resultLessonDto = _lessonService.UpdateLesson(lesson);

        if (resultLessonDto == null) return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(resultLessonDto);
    }

    public void Dispose()
    {
        _lessonService.Dispose();
    }
}
