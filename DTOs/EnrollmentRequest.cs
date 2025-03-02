using System.ComponentModel.DataAnnotations;

namespace Matrix;

public class EnrollmentRequest
{
    [ValidGuid(ErrorMessage = "UserId is a required field.")]
    public Guid UserId { get; set; }

    [ValidGuid(ErrorMessage = "CourseId is a required field.")]
    public Guid CourseId { get; set; }
}
