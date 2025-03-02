using System.ComponentModel.DataAnnotations;

namespace Matrix;

public class Credentials
{
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "The email field must be in correct e-mail address format.")] // RegEx for valid email formats
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
