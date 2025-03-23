using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class EnrollmentDao : IEnrollmentDao
{
    private MatrixCollegeContext _db;

    public EnrollmentDao(MatrixCollegeContext db)
    {
        _db = db;
    }

    public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
    {
        await _db.Enrollments.AddAsync(enrollment);

        await _db.SaveChangesAsync();

        return enrollment;
    }

    public async Task<List<Enrollment>> GetEnrollmentsByUserIdAsync(Guid userId)
    {
        return await _db.Enrollments.AsNoTracking().Where(enr => enr.UserId == userId).ToListAsync();
    }

    public async Task<bool> IsEnrollmentExists(Guid enrollmentId)
    {
        return await _db.Enrollments.AnyAsync(en => en.Id == enrollmentId);
    }

    public async Task<Enrollment?> GetEnrollmentByIdAsync(Guid enrollmentId)
    {
        return await _db.Enrollments.SingleOrDefaultAsync(e => e.Id == enrollmentId);
    }

    public async Task RemoveEnrollmentAsync(Guid id)
    {
        Enrollment enrollment = (await GetEnrollmentByIdAsync(id))!;
        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Enrollment>> GetEnrollmentsByCourseId(Guid id)
    {
        return await _db.Enrollments.Where(e => e.CourseId == id).ToListAsync();
    }
}
