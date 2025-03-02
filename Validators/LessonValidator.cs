using FluentValidation;
using System.Text.RegularExpressions;

namespace Matrix;

public class LessonValidator : AbstractValidator<Lesson>, IDisposable
{
    // DI
    private CourseService _courseService;

    public LessonValidator(CourseService courseService)
    {
        _courseService = courseService;

        // Not null and exists in DB
        RuleFor(lesson => lesson.CourseId).NotEmpty().WithMessage("The CourseId field is required.")
            .Must(CourseExists).WithMessage("No course was found for the provided courseId.");

        RuleFor(lesson => lesson.Title).NotNull().WithMessage("Title field is required.")
            .MinimumLength(5).WithMessage("Title must be at least 5 characters long.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters long.");

        RuleFor(lesson => lesson.VideoUrl).NotNull().WithMessage("VideoUrl field is required.")
            .Must(ValidUrl).WithMessage("VideoUrl must be a valid Url link.");
    }

    // Custom validations

    private bool ValidUrl(string url)
    {
        if (url == null)
            return false;

        return Regex.Match(url, "^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$").Success;
    }

    private bool CourseExists(Guid courseId)
    {
        return _courseService.IsCourseExists(courseId);
    }

    public void Dispose()
    {
        _courseService.Dispose();
    }
}
