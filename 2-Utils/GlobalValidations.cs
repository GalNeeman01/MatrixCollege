using System.Text.RegularExpressions;

namespace Matrix;

public static class GlobalValidations
{
    // Check for correct email format (RegEx)
    public static bool EmailFormat(string email)
    {
        if (email == null)
            return false;

        return Regex.Match(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").Success;
    }
}
