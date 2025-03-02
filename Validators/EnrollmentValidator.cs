using FluentValidation;

namespace Matrix;

public class EnrollmentValidator : AbstractValidator<Enrollment>, IDisposable
{
    // DI
    private CourseService _courseService;
    private UserService _userService;

    public EnrollmentValidator(CourseService courseService, UserService userService)
    {
        _courseService = courseService;
        _userService = userService;

        RuleFor(enrollment => enrollment.UserId).NotEmpty().WithMessage("UserId is a required field.")
            .Must(UserExists).WithMessage("No user was found for the provided UserId.");

        RuleFor(enrollment => enrollment.CourseId).NotEmpty().WithMessage("CourseId is a required field.")
            .Must(CourseExists).WithMessage("No course was found for the provided CourseId");
    }

    private bool CourseExists(Guid id)
    {
        return _courseService.IsCourseExists(id);
    }

    private bool UserExists(Guid id)
    {
        return _userService.IsUserExists(id);
    }

    public void Dispose()
    {
        _courseService.Dispose();
        _userService.Dispose();
    }
}
