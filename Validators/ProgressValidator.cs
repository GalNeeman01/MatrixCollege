using FluentValidation;

namespace Matrix;

public class ProgressValidator : AbstractValidator<Progress>, IDisposable
{
    // DI
    private LessonService _lessonService;
    private UserService _userService;

    public ProgressValidator(LessonService lessonService, UserService userService)
    {
        _lessonService = lessonService;
        _userService = userService;

        RuleFor(progress => progress.UserId).NotEmpty().WithMessage("UserId is a required field.")
            .Must(UserExists).WithMessage("No user was found for the provided UserId.");

        RuleFor(progress => progress.LessonId).NotEmpty().WithMessage("CourseId is a required field.")
            .Must(LessonExists).WithMessage("No lesson was found for the provided LessonId");
    }

    // Custom validations

    private bool LessonExists(Guid id)
    {
        return _lessonService.IsLessonExists(id);
    }

    private bool UserExists(Guid id)
    {
        return _userService.IsUserExists(id);
    }

    public void Dispose()
    {
        _lessonService.Dispose();
        _userService.Dispose();
    }
}
