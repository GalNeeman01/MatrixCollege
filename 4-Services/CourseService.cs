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

    public void Dispose()
    {
        _db.Dispose();
    }
}
