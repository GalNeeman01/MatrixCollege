using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class LessonController : ControllerBase, IDisposable
{
    private LessonService _lessonService;

    public LessonController(LessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpPost("/api/lessons")]
    public IActionResult AddLesson([FromBody] Lesson lesson)
    {
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
