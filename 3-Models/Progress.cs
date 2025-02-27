using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrix;

public class Progress
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; } // Foreign key to Users

    public Guid LessonId { get; set; } // Foreign key to Lessons

    public DateTime? WatchedAt { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("LessonId")]
    public Lesson Lesson { get; set; } = null!;
}
