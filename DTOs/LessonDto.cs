namespace Matrix;

public class LessonDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string VideoUrl { get; set; } = null!;
}
