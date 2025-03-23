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

        if (!(await _userService.IsUserExistsAsync(progressDto.UserId)))
            return BadRequest(new ResourceNotFoundError(progressDto.UserId.ToString()));

        if (!(await _lessonService.IsLessonExists(progressDto.LessonId)))
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
        return Ok(await _progressService.GetUserProgressDtoAsync(userId));
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

        if(!(await _userService.IsUserExistsAsync(enrollmentDto.UserId)))
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
        if (!(await _userService.IsUserExistsAsync(userId)))
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
        bool result = await _enrollmentService.RemoveEnrollmentAsync(enrollmentId);

        if (!result)
            return NotFound(new ResourceNotFoundError(enrollmentId.ToString()));

        return NoContent();
    }
}
