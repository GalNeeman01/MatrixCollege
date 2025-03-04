using FluentValidation;

namespace Matrix;

public class UserValidator : AbstractValidator<CreateUserDto>
{
    public UserValidator(UserService userService)
    {
        // required, min - 8, max - 50
        RuleFor(user => user.Name).NotNull().WithMessage("Name is a required field.")
            .MinimumLength(8).WithMessage("Name must be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters in length.");

        // required, min - 10, max - 320, email format, unique email.
        RuleFor(user => user.Email).NotNull().WithMessage("Email is a required field.")
            .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters in length.")
            .Must(GlobalValidations.EmailFormat).WithMessage("Email is in incorrect format.");

        // required, strong password
        RuleFor(user => user.Password).NotNull().WithMessage("Password is a required field.")
            .MaximumLength(800).WithMessage("Password cannot exceed 800 characters in length.")
            .Must(StrongPassword).WithMessage("Password must have 1 uppercase character, 1 digit and 1 non-alphanumeric character, and be at least 8 characters long.");
    }

    // Custom validations
    // Check for strong password.
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
