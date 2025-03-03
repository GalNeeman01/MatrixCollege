﻿using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
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

    [HttpGet("/api/lessons")]
    public IActionResult GetAllLessons()
    {
        List<LessonDto> lessons = _lessonService.GetAllLessons();

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
