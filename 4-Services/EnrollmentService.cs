using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class EnrollmentService : IDisposable
{
    private MatrixCollegeContext _db;

    public EnrollmentService(MatrixCollegeContext db)
    {
        _db = db;
    }

    // Check if both UserId and CourseId actually exist
    public bool IsEnrollmentValid(EnrollmentRequest enrollment)
    {
        // Fail if course doesn't exist
        if (!_db.Courses.AsNoTracking().Any(course => course.Id == enrollment.CourseId))
            return false;

        // Fail if user doesn't exist
        if (!_db.Users.AsNoTracking().Any(user => user.Id == enrollment.UserId))
            return false;

        return true;
    }

    public Enrollment Enroll(EnrollmentRequest enrollment)
    {
        DateTime now = DateTime.Now; // Store current time
        Enrollment dbEnrollment = new Enrollment { UserId = enrollment.UserId, CourseId = enrollment.CourseId, EnrolledAt = now }; // Create enrollment

        _db.Enrollments.Add(dbEnrollment);

        _db.SaveChanges();

        return dbEnrollment;
    }

    public List<Enrollment> GetAllEnrollments()
    {
        return _db.Enrollments.AsNoTracking().ToList();
    }

    public Enrollment? GetEnrollmentById(Guid enrollmentId)
    {
        return _db.Enrollments.AsNoTracking().SingleOrDefault(enr => enr.Id == enrollmentId);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
