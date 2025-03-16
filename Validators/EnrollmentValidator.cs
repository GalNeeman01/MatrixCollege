using FluentValidation;

namespace Matrix;

public class EnrollmentValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentValidator()
    {
        // UserId - required
        RuleFor(enrollment => enrollment.UserId).NotEmpty().WithMessage("UserId is a required field.");

        // CourseId - required
        RuleFor(enrollment => enrollment.CourseId).NotEmpty().WithMessage("CourseId is a required field.");
    }
}
