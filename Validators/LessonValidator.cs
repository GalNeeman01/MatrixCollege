using FluentValidation;

namespace Matrix;

public class LessonValidator : AbstractValidator<Lesson>, IDisposable
{
    private LessonService _lessonService;
}
