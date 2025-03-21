﻿using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("/api/v1/[Controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    // DI's
    // Services
    private IUserService _userService;
    private IEnrollmentService _enrollmentService;
    private IProgressService _progressService;
    private ICourseService _courseService;
    private ILessonService _lessonService;

    // Validators
    private IValidator<CreateUserDto> _userValidator;
    private IValidator<ProgressDto> _progressValidator;
    private IValidator<EnrollmentDto> _enrollmentValidator;
    private IValidator<Credentials> _credentialsValidator;

    // Mappers
    private IMapper _mapper;

    // Constructor
    public UsersController(
        IUserService userService, 
        IEnrollmentService enrollmentService,
        IProgressService progressService,
        ICourseService courseService,
        ILessonService lessonService,
        IValidator<CreateUserDto> userValidator, 
        IValidator<ProgressDto> progressValidator, 
        IValidator<EnrollmentDto> enrollmentValidator,
        IValidator<Credentials> credentialsValidator,
        IMapper mapper)
    {
        _userService = userService;
        _courseService = courseService;
        _lessonService = lessonService;
        _userValidator = userValidator;
        _progressValidator = progressValidator;
        _enrollmentValidator = enrollmentValidator;
        _credentialsValidator = credentialsValidator;
        _enrollmentService = enrollmentService;
        _progressService = progressService;
        _mapper = mapper;
    }

    // Routes
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto userDto)
    {
        // Fluent validation
        ValidationResult validationResult = _userValidator.Validate(userDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        if (!(await _userService.IsEmailUniqueAsync(userDto.Email)))
            return BadRequest(new ValidationError<string>("Email is already taken."));

        // Map to User object
        User user = _mapper.Map<User>(userDto);

        string token = await _userService.RegisterAsync(user);

        // Return an object so Front-End can parse it as json
        return Created("/", token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Credentials credentials)
    {
        // Fluent validation
        ValidationResult validationResult = _credentialsValidator.Validate(credentials);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));


        string? token = await _userService.LoginAsync(credentials);

        if (token == null)
            return BadRequest(new ValidationError<string>("Incorrect email or password."));

        // Return an object so Front-End can parse it as json
        return Ok(token);
    }

    // Progress routes
    [Authorize(Roles = "Student")]
    [HttpPost("progress")]
    public async Task<IActionResult> AddProgressAsync([FromBody] ProgressDto progressDto)
    {
        if (progressDto == null)
            return BadRequest(new RequestDataError());

        if (!_userService.IsUserExists(progressDto.UserId))
            return BadRequest(new ResourceNotFoundError(progressDto.UserId.ToString()));

        if (!_lessonService.IsLessonExists(progressDto.LessonId))
            return BadRequest(new ResourceNotFoundError(progressDto.LessonId.ToString()));

        ValidationResult validationResult = _progressValidator.Validate(progressDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        Progress progress = _mapper.Map<Progress>(progressDto);
        ProgressDto resultProgress = await _progressService.AddProgressAsync(progress);

        return Created("/", resultProgress);
    }

    [Authorize(Roles = "Student")]
    [HttpGet("progress/{userId}")]
    public async Task<IActionResult> GetUserProgressAsync([FromRoute] Guid userId)
    {
        return Ok(await _progressService.GetUserProgressAsync(userId));
    }

    [Authorize(Roles = "Student")]
    [HttpPost("enrollments")]
    public async Task<IActionResult> AddEnrollmentAsync([FromBody] EnrollmentDto enrollmentDto)
    {
        // In case of invalid Guid from request which would cause a crash
        if (enrollmentDto == null)
            return BadRequest(new RequestDataError());

        if (await _courseService.IsCourseExistsAsync(enrollmentDto.CourseId) == false)
            return BadRequest(new ResourceNotFoundError(enrollmentDto.CourseId.ToString()));

        if(!_userService.IsUserExists(enrollmentDto.UserId))
            return BadRequest(new ResourceNotFoundError(enrollmentDto.UserId.ToString()));

        // Fluent validation
        ValidationResult validationResult = _enrollmentValidator.Validate(enrollmentDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        // Map to Enrollment
        Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentDto);

        // Call to service
        EnrollmentDto resultEnrollment = await _enrollmentService.EnrollAsync(enrollment);

        return Created("/", resultEnrollment);
    }

    [Authorize(Roles = "Student")]
    [HttpGet("enrollments/{userId}")]
    public async Task<IActionResult> GetUserEnrollmentsAsync([FromRoute] Guid userId)
    {
        if (!_userService.IsUserExists(userId))
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        List<EnrollmentDto> dtoEnrollments = await _enrollmentService.GetEnrollmentsByUserIdAsync(userId);

        if (dtoEnrollments == null)
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        return Ok(dtoEnrollments);
    }

    [Authorize(Roles = "Student")]
    [HttpDelete("enrollments/{enrollmentId}")]
    public async Task<IActionResult> RemoveEnrollmentAsync([FromRoute] Guid enrollmentId)
    {
        if (!(await _enrollmentService.IsEnrollmentExists(enrollmentId)))
            return NotFound(new ResourceNotFoundError(enrollmentId.ToString()));

        bool result = await _enrollmentService.RemoveEnrollmentAsync(enrollmentId);

        if (!result)
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new GeneralError("Some error occured.. Please try again later"));

        return NoContent();
    }
}
