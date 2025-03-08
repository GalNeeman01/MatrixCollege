namespace Matrix;

public class ProgressDto
{
    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    public DateTime? WatchedAt { get; set; }
}
