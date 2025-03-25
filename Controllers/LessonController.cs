using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("/api/v1/[Controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    // DI's
    private ILessonService _lessonService;
    private IValidator<LessonDto> _validator;

    // Constructor
    public LessonsController(ILessonService lessonService, IValidator<LessonDto> validator)
    {
        _lessonService = lessonService;
        _validator = validator;
    }

    // Routes
    [Authorize(Roles = "Professor")]
    [HttpPost]
    public async Task<IActionResult> AddLessonsAsync([FromBody] List<LessonDto> lessonDtos)
    {
        // If all lessons are validated, call DB to add them
        List<LessonDto>? result = await _lessonService.AddLessonsAsync(lessonDtos);

        if (result == null)
            return NotFound(new GeneralError("No corresponding courses found for some of the lessons"));

        return Created("/", result);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet]
    public async Task<IActionResult> GetAllLessonsAsync()
    {
        List<LessonDto> lessons = await _lessonService.GetAllLessonsDtoAsync();

        return Ok(lessons);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLessonByIdAsync([FromRoute] Guid lessonId)
    {
        LessonDto? lesson = await _lessonService.GetLessonDtoByIdAsync(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetLessonsByCourseIdAsync([FromRoute] Guid courseId)
    {
        List<LessonDto>? lessons = await _lessonService.GetLessonsDtoByCourseIdAsync(courseId);

        if (lessons == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(lessons);
    }

    [HttpGet("course/{courseId}/preview")]
    public async Task<IActionResult> GetLessonsInfoByCourseIdAsync([FromRoute] Guid courseId)
    {
        List<LessonInfoDto>? lessons = await _lessonService.GetLessonsInfoByCourseIdAsync(courseId);

        if (lessons == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(lessons);
    }

    [Authorize(Roles = "Professor")]
    [HttpPost("delete")] // Must be post to accept a list of items
    public async Task<IActionResult> RemoveLessonsAsync([FromBody] List<Guid> lessonIds)
    {
        bool result = await _lessonService.RemoveLessonsAsync(lessonIds);

        if (!result)
            return NotFound(new GeneralError("No lessons with matching IDs were found."));

        return NoContent();
    }

    [Authorize(Roles = "Professor")]
    [HttpPut]
    public async Task<IActionResult> UpdateLessonsAsync([FromBody] List<LessonDto> lessonDtos)
    {
        // Call to service after each lesson was validated
        List<LessonDto>? resultLessonsDto = await _lessonService.UpdateLessonsAsync(lessonDtos);

        if (resultLessonsDto == null)
            return NotFound(new GeneralError("One of the provided lessons does not exist."));

        return Ok(resultLessonsDto);
    }
}
