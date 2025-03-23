using AutoMapper;
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
        IValidator<Credentials> credentialsValidator)
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
    }

    // Routes
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto userDto)
    {
        // Fluent validation
        ValidationResult validationResult = _userValidator.Validate(userDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        string? token = await _userService.RegisterAsync(userDto);

        // Service will return null if the email already exists
        if (token == null)
            return BadRequest(new ValidationError<string>("Email is already taken."));

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

        // Service will return null for incorrect credentials
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

        ValidationResult validationResult = _progressValidator.Validate(progressDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        ProgressDto? resultProgress = await _progressService.AddProgressAsync(progressDto);

        if (resultProgress == null)
            return NotFound(new GeneralError("Either the provided course or the provided user do not exist."));

        return Created("/", resultProgress);
    }

    [Authorize(Roles = "Student")]
    [HttpGet("progress/{userId}")]
    public async Task<IActionResult> GetUserProgressAsync([FromRoute] Guid userId)
    {
        List<ProgressDto>? progresses = await _progressService.GetUserProgressDtoAsync(userId);

        if (progresses == null)
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        return Ok(progresses);
    }

    [Authorize(Roles = "Student")]
    [HttpPost("enrollments")]
    public async Task<IActionResult> AddEnrollmentAsync([FromBody] EnrollmentDto enrollmentDto)
    {
        // In case of invalid Guid from request which would cause a crash
        if (enrollmentDto == null)
            return BadRequest(new RequestDataError());

        // Fluent validation
        ValidationResult validationResult = _enrollmentValidator.Validate(enrollmentDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError<List<string>>(validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        
        // Call service
        EnrollmentDto? resultEnrollment = await _enrollmentService.EnrollAsync(enrollmentDto);

        if (resultEnrollment == null)
            return NotFound(new GeneralError("Either the provided course or the provided user do not exist."));

        return Created("/", resultEnrollment);
    }

    [Authorize(Roles = "Student")]
    [HttpGet("enrollments/{userId}")]
    public async Task<IActionResult> GetUserEnrollmentsAsync([FromRoute] Guid userId)
    {
        List<EnrollmentDto>? dtoEnrollments = await _enrollmentService.GetEnrollmentsByUserIdAsync(userId);

        if (dtoEnrollments == null) // Will receive null from service if no user was found for given ID
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        return Ok(dtoEnrollments);
    }

    [Authorize(Roles = "Student")]
    [HttpDelete("enrollments/{enrollmentId}")]
    public async Task<IActionResult> RemoveEnrollmentAsync([FromRoute] Guid enrollmentId)
    {
        bool result = await _enrollmentService.RemoveEnrollmentAsync(enrollmentId);

        if (!result)
            return NotFound(new ResourceNotFoundError(enrollmentId.ToString()));

        return NoContent();
    }
}
