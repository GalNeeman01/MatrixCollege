using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrix;

public class Progress
{
    [Key]
    public Guid Id { get; set; }

    //[ValidGuid(ErrorMessage = "UserId is a required field.")]
    public Guid UserId { get; set; } // Foreign key to Users

    //[ValidGuid(ErrorMessage = "LessonId is a required field.")]
    public Guid LessonId { get; set; } // Foreign key to Lessons

    public DateTime? WatchedAt { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("LessonId")]
    public Lesson Lesson { get; set; } = null!;
}
