using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[ApiController]
public class UserController : ControllerBase, IDisposable
{
    // DI
    private UserService _userService;
    private EnrollmentService _enrollmentService;
    private ProgressService _progressService;

    private IValidator<User> _userValidator;
    private IValidator<Progress> _progressValidator;
    private IValidator<Enrollment> _enrollmentValidator;
    private IValidator<Credentials> _credentialsValidator;

    // Constructor
    public UserController(
        UserService userService, 
        EnrollmentService enrollmentService,
        ProgressService progressService,
        IValidator<User> userValidator, 
        IValidator<Progress> progressValidator, 
        IValidator<Enrollment> enrollmentValidator,
        IValidator<Credentials> credentialsValidator)
    {
        _userService = userService;
        _userValidator = userValidator;
        _progressValidator = progressValidator;
        _enrollmentValidator = enrollmentValidator;
        _credentialsValidator = credentialsValidator;
        _enrollmentService = enrollmentService;
        _progressService = progressService;
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
    public IActionResult AddProgress([FromBody] Progress progress)
    {
        if (progress == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _progressValidator.Validate(progress);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));

        Progress dbProgress = _progressService.AddProgress(progress);

        return Created("/", dbProgress);
    }

    [HttpGet("/api/user-progress/{userId}")]
    public IActionResult GetUserProgress([FromRoute] Guid userId)
    {
        return Ok(_progressService.GetUserProgress(userId));
    }

    // Enrollment routes
    [HttpPost("/api/user-enroll")]
    public IActionResult AddEnrollment([FromBody] Enrollment enrollment)
    {
        // In case of invalid Guid from request which would cause a crash
        if (enrollment == null)
            return BadRequest(new RequestDataError());

        // Fluent validation
        ValidationResult validationResult = _enrollmentValidator.Validate(enrollment);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));


        Enrollment dbEnrollment = _enrollmentService.Enroll(enrollment);

        return Created("/", dbEnrollment);
    }

    [HttpGet("/api/user-enrollments/{userId}")]
    public IActionResult GetUserEnrollments([FromRoute] Guid userId)
    {
        if (!_userService.IsUserExists(userId))
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        List<Enrollment> enrollment = _enrollmentService.GetEnrollmentsByUserId(userId);

        if (enrollment == null)
            return NotFound(new ResourceNotFoundError(userId.ToString()));

        return Ok(enrollment);
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
