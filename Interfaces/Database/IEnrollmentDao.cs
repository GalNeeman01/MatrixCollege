namespace Matrix;

public interface IEnrollmentDao
{
    public Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);

    public Task<List<Enrollment>> GetEnrollmentsByUserIdAsync(Guid userId);

    public Task<bool> IsEnrollmentExists(Guid enrollmentId);

    public Task RemoveEnrollmentAsync(Guid id);

    public Task<Enrollment?> GetEnrollmentByIdAsync(Guid enrollmentId);

    public Task<List<Enrollment>> GetEnrollmentsByCourseId(Guid id);
}
