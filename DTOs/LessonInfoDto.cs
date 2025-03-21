namespace Matrix;

public class LessonInfoDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;
}
