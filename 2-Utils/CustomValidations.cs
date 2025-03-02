using System.ComponentModel.DataAnnotations;

namespace Matrix;

public class StrongPassword : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // Fail if the input is invalid
        if (value == null || value is not string)
            return false;

        string password = Convert.ToString(value)!;

        // Fail if password is too short
        if (password.Length < 8)
            return false;

        // Fail if: no numeric characters | no characters | non non-alphanumeric characters
        if (!password.Any(c => char.IsUpper(c)) ||
            !password.Any(c => char.IsDigit(c)) ||
            !password.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        // Passed
        return true;
    }
}

public class ValidGuid : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // Fail if incorrect type or null
        if (value == null || value is not Guid)
            return false;

        Guid id = (Guid)value;

        // Fail if empty (initialized null)
        if (id == Guid.Empty)
            return false;

        // Passed
        return true;
    }
}
