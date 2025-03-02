using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class CourseService : IDisposable
{
    private MatrixCollegeContext _db;

    public CourseService(MatrixCollegeContext db)
    {
        _db = db;
    }

    public Course CreateCourse(Course course)
    {
        course.CreatedAt = DateTime.Now; // Set the current time

        _db.Courses.Add(course);

        _db.SaveChanges();

        return course;
    }

    public List<Course> GetAllCourses()
    {
        return _db.Courses.AsNoTracking().ToList();
    }

    public Course? GetCourseById(Guid courseId)
    {
        return _db.Courses.AsNoTracking().SingleOrDefault(course => course.Id == courseId);
    }

    // Return whether a course exists in the DB
    public bool IsCourseExists(Guid courseId)
    {
        return _db.Courses.AsNoTracking().Any(course => course.Id == courseId);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
