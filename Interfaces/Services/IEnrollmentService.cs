﻿namespace Matrix;

public interface IEnrollmentService
{
    public Task<EnrollmentDto?> EnrollAsync(EnrollmentDto enrollment);

    public Task<List<EnrollmentDto>?> GetEnrollmentsByUserIdAsync(Guid userId);

    public Task<bool> RemoveEnrollmentAsync(Guid id);

    public Task RemoveEnrollmentsByCourseAsync(Guid courseId);
}
