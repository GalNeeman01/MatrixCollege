namespace Matrix;

public class EnrollmentDto
{
    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public DateTime EnrolledAt { get; set; }
}
