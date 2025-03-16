using FluentValidation;

namespace Matrix;

public class UserValidator : AbstractValidator<CreateUserDto>
{
    public UserValidator()
    {
        // Name - required, 8 - 50 characters
        RuleFor(user => user.Name).NotNull().WithMessage("Name is a required field.")
            .MinimumLength(8).WithMessage("Name must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters in length.");

        // Email - required, 10 - 320 characters, email format, unique email.
        RuleFor(user => user.Email).NotNull().WithMessage("Email is a required field.")
            .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters in length.")
            .Must(GlobalValidations.EmailFormat).WithMessage("Email is in incorrect format.");

        // Password - required, strong password (at least: 8 characters, 1 uppercase, 1 digit, 1 non-alphanumeric)
        RuleFor(user => user.Password).NotNull().WithMessage("Password is a required field.")
            .MaximumLength(800).WithMessage("Password cannot exceed 800 characters in length.")
            .Must(StrongPassword).WithMessage("Password must have 1 uppercase character, 1 digit and 1 non-alphanumeric character, and be at least 8 characters long.");
    }

    // Custom validations
    public bool StrongPassword(string password)
    {
        if (password == null)
            return false;

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
