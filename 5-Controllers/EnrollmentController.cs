using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private EnrollmentService _enrollmentService;

    public EnrollmentController(EnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost("/api/enrollments")]
    public IActionResult AddEnrollment([FromBody] EnrollmentRequest enrollment)
    {
        Enrollment dbEnrollment = _enrollmentService.Enroll(enrollment);

        if (!_enrollmentService.IsEnrollmentValid(enrollment))
            return BadRequest(new ValidationError("Incorrect UserId or CourseId"));

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
}
