using System.ComponentModel.DataAnnotations;

namespace Matrix;

public class EnrollmentRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid CourseId { get; set; }
}
