using FluentValidation;

namespace Matrix;

public class CourseValidator : AbstractValidator<CourseDto>
{
    public CourseValidator()
    {
        // Title - Required, 2 - 100 characters
        RuleFor(course => course.Title).NotNull().WithMessage("Title is a required field.")
            .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters in length.");

        // Description - Required, max 3000 characters
        RuleFor(course => course.Description).NotNull().WithMessage("Description is a required field.")
            .MaximumLength(3000).WithMessage("Description cannot exceed 3000 characters in length.");
    }
}
