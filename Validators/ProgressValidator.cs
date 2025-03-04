using FluentValidation;

namespace Matrix;

public class ProgressValidator : AbstractValidator<ProgressDto>
{
    public ProgressValidator()
    {
        RuleFor(progress => progress.UserId).NotEmpty().WithMessage("UserId is a required field.");

        RuleFor(progress => progress.LessonId).NotEmpty().WithMessage("CourseId is a required field.");
    }
}
