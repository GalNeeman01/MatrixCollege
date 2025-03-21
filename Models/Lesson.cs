using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Matrix;

public class Lesson
{
    // Fields
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("Courses")]
    public Guid CourseId { get; set; }  // Foreign key to Courses

    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    public string VideoUrl { get; set; } = null!;

    [InverseProperty("Lessons")]
    [JsonIgnore]
    public Course? Course { get; set; } = null!; // One to many relation
}
