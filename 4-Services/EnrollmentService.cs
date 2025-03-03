using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class EnrollmentService : IDisposable
{
    private MatrixCollegeContext _db;

    public EnrollmentService(MatrixCollegeContext db)
    {
        _db = db;
    }

    public Enrollment Enroll(Enrollment enrollment)
    {
        DateTime now = DateTime.Now; // Store current time
        Enrollment dbEnrollment = new Enrollment { UserId = enrollment.UserId, CourseId = enrollment.CourseId, EnrolledAt = now }; // Create enrollment

        _db.Enrollments.Add(dbEnrollment);

        _db.SaveChanges();

        return dbEnrollment;
    }

    public List<Enrollment> GetEnrollmentsByUserId(Guid userId)
    {
        return _db.Enrollments.AsNoTracking().Where(enr => enr.UserId == userId).ToList();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
