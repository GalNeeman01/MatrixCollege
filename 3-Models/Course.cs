using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Matrix;

public class Course
{
    // Fields
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage="Title must be at least 5 characters in length.")]
    [MaxLength(100, ErrorMessage="Title cannot exceed 100 characters in length.")]
    public string Title { get; set; } = null!;

    [Required]
    [MinLength(0)]
    [MaxLength(3000, ErrorMessage ="Description cannot exceed 3000 characters in length.")]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Course")]
    [JsonIgnore]
    public List<Lesson>? Lessons { get; set; } // One to many relation

    [InverseProperty("Course")]
    [JsonIgnore]
    public List<Enrollment>? Enrollments { get; set; } // One to many relation
}
