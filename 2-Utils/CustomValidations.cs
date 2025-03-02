using System.ComponentModel.DataAnnotations;

namespace Matrix;

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
