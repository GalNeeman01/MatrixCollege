using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Matrix;

public class Lesson
{
    // Fields
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Courses")]
    [Required]
    public Guid CourseId { get; set; }  // Foreign key to Courses

    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters in length.")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters in length.")]
    public string Title { get; set; } = null!;

    [Required]
    [RegularExpression("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$",
                        ErrorMessage = "Url is in incorrect format.")] // Url RegEx
    public string VideoUrl { get; set; } = null!;

    [InverseProperty("Lessons")]
    [JsonIgnore]
    public Course Course { get; set; } = null!; // One to many relation
}
