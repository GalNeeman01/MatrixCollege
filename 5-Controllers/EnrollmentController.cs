using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase, IDisposable
{
    // DI
    private EnrollmentService _enrollmentService;
    private EnrollmentValidator _validator;

    public EnrollmentController(EnrollmentService enrollmentService, EnrollmentValidator validatior)
    {
        _enrollmentService = enrollmentService;
        _validator = validatior;
    }

    [HttpPost("/api/enrollments")]
    public IActionResult AddEnrollment([FromBody] Enrollment enrollment)
    {
        // In case of invalid Guid from request which would cause a crash
        if (enrollment == null)
            return BadRequest(new RequestDataError());

        ValidationResult validationResult = _validator.Validate(enrollment);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));


        Enrollment dbEnrollment = _enrollmentService.Enroll(enrollment);

        return Created("/", dbEnrollment);
    }

    [HttpGet("/api/enrollments")]
    public IActionResult GetAllEnrollments()
    {
        return Ok(_enrollmentService.GetAllEnrollments());
    }

    [HttpGet("/api/enrollments/{enrollmentId}")]
    public IActionResult GetEnrollmentById([FromRoute] Guid enrollmentId)
    {
        Enrollment? enrollment = _enrollmentService.GetEnrollmentById(enrollmentId);

        if (enrollment == null)
            return NotFound(new ResourceNotFoundError(enrollmentId.ToString()));

        return Ok(enrollment);
    }

    public void Dispose()
    {
        _enrollmentService.Dispose();
    }

}
