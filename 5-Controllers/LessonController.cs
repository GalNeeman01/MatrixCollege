using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class LessonController : ControllerBase, IDisposable
{
    private LessonService _lessonService;
    private IValidator<Lesson> _validator;

    public LessonController(LessonService lessonService, IValidator<Lesson> validator)
    {
        _lessonService = lessonService;
        _validator = validator;
    }
    
    [HttpPost("/api/lessons")]
    public IActionResult AddLesson([FromBody] Lesson lesson)
    {
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lesson == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(lesson);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        Lesson dbLesson = _lessonService.AddLesson(lesson);

        return Created("/", dbLesson);
    }

    [HttpGet("/api/lessons")]
    public IActionResult GetAllLessons()
    {
        List<Lesson> lessons = _lessonService.GetAllLessons();

        return Ok(lessons);
    }

    [HttpGet("/api/lessons/{lessonId}")]
    public IActionResult GetLessonById([FromRoute] Guid lessonId)
    {
        Lesson? lesson = _lessonService.GetLessonById(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }


    public void Dispose()
    {
        _lessonService.Dispose();
    }
}
