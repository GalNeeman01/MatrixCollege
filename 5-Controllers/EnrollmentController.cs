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
}
