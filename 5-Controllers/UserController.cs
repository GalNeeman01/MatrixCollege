using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[ApiController]
public class UserController : ControllerBase, IDisposable
{
    // DI
        // Services
    private UserService _userService;
    private EnrollmentService _enrollmentService;
    private ProgressService _progressService;

        // Validators
    private IValidator<User> _userValidator;
    private IValidator<ProgressDto> _progressValidator;
    private IValidator<EnrollmentDto> _enrollmentValidator;
    private IValidator<Credentials> _credentialsValidator;

    // Mappers
    private IMapper _mapper;

    // Constructor
    public UserController(
        UserService userService, 
        EnrollmentService enrollmentService,
        ProgressService progressService,
        IValidator<User> userValidator, 
        IValidator<ProgressDto> progressValidator, 
        IValidator<EnrollmentDto> enrollmentValidator,
        IValidator<Credentials> credentialsValidator,
        IMapper mapper)
    {
        _userService = userService;
        _userValidator = userValidator;
        _progressValidator = progressValidator;
        _enrollmentValidator = enrollmentValidator;
        _credentialsValidator = credentialsValidator;
        _enrollmentService = enrollmentService;
        _progressService = progressService;
        _mapper = mapper;
    }

    // Routes
    [HttpPost("/api/register")]
    public IActionResult Register([FromBody] User user)
    {
        // Fluent validation
        ValidationResult validationResult = _userValidator.Validate(user);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        User dbUser = _userService.Register(user);
        return Created("/", dbUser);
    }

    [HttpPost("/api/login")]
    public IActionResult Login([FromBody] Credentials credentials)
    {
        // Fluent validation
        ValidationResult validationResult = _credentialsValidator.Validate(credentials);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));


        User? dbUser = _userService.Login(credentials);

        if (dbUser == null)
            return BadRequest(new ValidationError("Incorrect email or password."));

        return Ok(dbUser);
    }

    // Progress routes
    [HttpPost("/api/user-progress")]
    public IActionResult AddProgress([FromBody] ProgressDto progressDto)
    {
        if (progressDto == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _progressValidator.Validate(progressDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        Progress progress = _mapper.Map<Progress>(progressDto);
        ProgressDto resultProgress = _progressService.AddProgress(progress);

        return Created("/", resultProgress);
    }

    [HttpGet("/api/user-progress/{userId}")]
    public IActionResult GetUserProgress([FromRoute] Guid userId)
    {
        return Ok(_progressService.GetUserProgress(userId));
    }

    // Enrollment routes
    [HttpPost("/api/user-enroll")]
    public IActionResult AddEnrollment([FromBody] EnrollmentDto enrollmentDto)
    {
        // In case of invalid Guid from request which would cause a crash
        if (enrollmentDto == null)
            return BadRequest(new RequestDataError());

        // Fluent validation
        ValidationResult validationResult = _enrollmentValidator.Validate(enrollmentDto);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        // Map to Enrollment
        Enrollment enrollment = _mapper.Map<Enrollment>(enrollmentDto);

        // Call to service
        EnrollmentDto resultEnrollment = _enrollmentService.Enroll(enrollment);

        return Created("/", resultEnrollment);
    }

    [HttpGet("/api/user-enrollments/{userId}")]
    public IActionResult GetUserEnrollments([FromRoute] Guid userId)
    {
        if (!_userService.IsUserExists(userId))
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        List<EnrollmentDto> dtoEnrollments = _enrollmentService.GetEnrollmentsByUserId(userId);

        if (dtoEnrollments == null)
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        return Ok(dtoEnrollments);
    }

    [HttpDelete("/api/user-enrollments/{enrollmentId}")]
    public IActionResult RemoveEnrollment([FromRoute] Guid enrollmentId)
    {
        bool result = _enrollmentService.RemoveEnrollment(enrollmentId);

        if (!result)
            return NotFound(new ResourceNotFoundError(enrollmentId.ToString()));

        return NoContent();
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _userService.Dispose();
    }
}
