namespace Matrix;

public interface IEnrollmentService
{
    public Task<EnrollmentDto> EnrollAsync(Enrollment enrollment);

    public Task<List<EnrollmentDto>> GetEnrollmentsByUserIdAsync(Guid userId);

    public Task<bool> IsEnrollmentExists(Guid enrollmentId);

    public Task<bool> RemoveEnrollmentAsync(Guid id);

    public Task RemoveEnrollmentsByCourseAsync(Guid courseId);
}
