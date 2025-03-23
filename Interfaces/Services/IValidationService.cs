namespace Matrix;

public interface IValidationService
{
    public Task<bool> IsLessonExistsAsync(Guid lessonId);

    public Task<bool> IsCourseExistsAsync(Guid courseId);

    public Task<bool> IsUserExistsAsync(Guid userId);

    public Task<bool> IsEnrollmentExistsAsync(Guid enrollmentId);
}
