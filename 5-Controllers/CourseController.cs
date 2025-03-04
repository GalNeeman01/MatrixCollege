﻿using AutoMapper;
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
    public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
    {
        // Fluent validation
        ValidationResult validationResult = _validator.Validate(courseDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map Dto to Course object
        Course course = _mapper.Map<Course>(courseDto);

        // Retreive created courseDto
        CourseDto createdCourse = await _courseService.CreateCourse(course);

        return Created("/", createdCourse);
    }

    [HttpGet("/api/courses")]
    public async Task<IActionResult> GetAllCourses()
    {
        List<CourseDto> courses = await _courseService.GetAllCourses();

        return Ok(courses);
    }

    [HttpGet("/api/courses/{courseId}")]
    public async Task<IActionResult> GetCourseById([FromRoute] Guid courseId)
    {
        CourseDto? course = await _courseService.GetCourseById(courseId);

        if (course == null)
            return NotFound(new ResourceNotFoundError(courseId.ToString()));

        return Ok(course);
    }

    [Authorize(Roles = "Professor")]
    [HttpDelete("/api/courses/{courseId}")]
    public async Task<IActionResult> RemoveCourse([FromRoute] Guid courseId)
    {
        bool result = await _courseService.RemoveCourse(courseId);

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
