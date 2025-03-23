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
    private ICourseService _courseService;
    private IMapper _mapper;
    private IValidator<LessonDto> _validator;

    // Constructor
    public LessonsController(ILessonService lessonService, IValidator<LessonDto> validator, IMapper mapper, ICourseService courseService)
    {
        _lessonService = lessonService;
        _validator = validator;
        _mapper = mapper;
        _courseService = courseService;
    }

    // Routes
    [Authorize(Roles = "Professor")]
    [HttpPost]
    public async Task<IActionResult> AddLessonsAsync([FromBody] List<LessonDto> lessonDtos)
    {
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDtos == null || lessonDtos.Count == 0) // No data recieved
            return BadRequest(new RequestDataError());

        // Validate each item to add
        foreach (LessonDto lessonDto in lessonDtos)
        {
            ValidationResult validationResult = _validator.Validate(lessonDto);

            if (!validationResult.IsValid)
                return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

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
        List<LessonDto> lessons = await _lessonService.GetAllLessonsAsync();

        return Ok(lessons);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLessonByIdAsync([FromRoute] Guid lessonId)
    {
        LessonDto? lesson = await _lessonService.GetLessonByIdAsync(lessonId);

        // If no lesson with given id exists in DB
        if (lesson == null)
            return NotFound(new ResourceNotFoundError(lessonId.ToString()));

        return Ok(lesson);
    }

    [Authorize(Roles = "Professor,Student")]
    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetLessonsByCourseIdAsync([FromRoute] Guid courseId)
    {
        return Ok(await _lessonService.GetLessonsDtoByCourseIdAsync(courseId));
    }

    [HttpGet("course/{courseId}/preview")]
    public async Task<IActionResult> GetLessonsInfoByCourseIdAsync([FromRoute] Guid courseId)
    {
        return Ok(await _lessonService.GetLessonsInfoByCourseIdAsync(courseId));
    }

    [Authorize(Roles = "Professor")]
    [HttpPost("delete")] // Must be post to accept a list of items
    public async Task<IActionResult> RemoveLessonsAsync([FromBody] List<Guid> lessonIds)
    {
        if (lessonIds == null || lessonIds.Count() == 0) // No data recieved
            return BadRequest(new RequestDataError());

        bool result = await _lessonService.RemoveLessonsAsync(lessonIds);

        if (!result)
            return NotFound(new GeneralError("No lessons with matching IDs were found."));

        return NoContent();
    }

    [Authorize(Roles = "Professor")]
    [HttpPut]
    public async Task<IActionResult> UpdateLessonsAsync([FromBody] List<LessonDto> lessonDtos)
    {
        // Fluent validation on DTO:
        // Make sure lesson was created successfully since if it receives an empty Guid it will fail to create and result in null
        if (lessonDtos == null || lessonDtos.Count == 0)
            return BadRequest(new RequestDataError());

        foreach (LessonDto lessonDto in lessonDtos)
        {
            if (!(await _lessonService.IsLessonExists(lessonDto.Id)))
                return NotFound(new ResourceNotFoundError(lessonDto.Id.ToString()));

            ValidationResult validationResult = _validator.Validate(lessonDto);

            if (!validationResult.IsValid)
                return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        // Call to service after each lesson was validated
        List<LessonDto> resultLessonsDto = await _lessonService.UpdateLessonsAsync(lessonDtos);

        return Ok(resultLessonsDto);
    }
}
