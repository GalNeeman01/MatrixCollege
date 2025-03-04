using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Matrix;
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    [InverseProperty("Users")]
    [JsonIgnore]
    public Role Role { get; set; } = null!;

    [Required]
    [MinLength(8)]
    [MaxLength(800)]
    public string Password { get; set; } = null!;

    [InverseProperty("User")]
    [JsonIgnore]
    public List<Enrollment>? Enrollments { get; set; } // One to many relation
}
