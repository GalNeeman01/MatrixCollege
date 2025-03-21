using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Matrix;

public class Enrollment
{
    // Fields
    [Key]
    public Guid Id { get; set; }

    //[ValidGuid(ErrorMessage = "UserId is a required field.")]
    [ForeignKey("Users")]
    public Guid UserId { get; set; } // Foreign key to Users

    //[ValidGuid(ErrorMessage = "CourseId is a required field.")]
    [ForeignKey("Courses")]
    public Guid CourseId { get; set; } // Foreign key to Courses

    [Required]
    public DateTime EnrolledAt { get; set; } // Auto generate

    [InverseProperty("Enrollments")]
    [JsonIgnore]
    public User User { get; set; } = null!; // One to many relation

    [InverseProperty("Enrollments")]
    [JsonIgnore]
    public Course Course { get; set; } = null!; // One to many relation
}
