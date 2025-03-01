﻿using Microsoft.EntityFrameworkCore;
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
    [MinLength(8, ErrorMessage = "Name must be at least 8 characters long.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters in length.")]
    public string Name { get; set; } = null!;

    [Required]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "The email field must be in correct e-mail address format.")] // RegEx for valid email formats
    [MinLength(10, ErrorMessage = "Email must be at least 10 characters long.")]
    [MaxLength(320, ErrorMessage = "Email cannot exceed 320 characters in length.")]
    public string Email { get; set; } = null!;

    [Required]
    [StrongPassword(ErrorMessage = "Password must have at least: 1 digit, 1 characters, 1 non-alphanumeric character, and be at least 8 characters long.")]
    [MaxLength(800, ErrorMessage = "Password cannot exceed 800 characters in length.")]
    public string Password { get; set; } = null!;

    [InverseProperty("User")]
    [JsonIgnore]
    public List<Enrollment>? Enrollments { get; set; } // One to many relation
}
