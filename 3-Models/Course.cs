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
    [MinLength(2)]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(3000)]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } // Auto generated

    [InverseProperty("Course")]
    [JsonIgnore]
    public List<Lesson>? Lessons { get; set; } // One to many relation

    [InverseProperty("Course")]
    [JsonIgnore]
    public List<Enrollment>? Enrollments { get; set; } // One to many relation
}
