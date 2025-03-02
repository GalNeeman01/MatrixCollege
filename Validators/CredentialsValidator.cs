﻿using FluentValidation;

namespace Matrix;

public class CredentialsValidator : AbstractValidator<Credentials>
{
    public CredentialsValidator()
    {
        RuleFor(credentials => credentials.Email).NotNull().WithMessage("Email feild is required.")
            .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters in length.")
            .Must(GlobalValidations.EmailFormat).WithMessage("Email is in incorrect format.");

        RuleFor(credentials => credentials.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(800).WithMessage("Password cannot exceed 800 characters in length.");
    }
}
