

using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class CourseDao : ICourseDao
{
    private MatrixCollegeContext _db;

    public CourseDao(MatrixCollegeContext db)
    {
        _db = db;
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        await _db.Courses.AddAsync(course);

        await _db.SaveChangesAsync();

        return course;
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _db.Courses.AsNoTracking().ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(Guid courseId)
    {
        return await _db.Courses.AsNoTracking().SingleOrDefaultAsync(course => course.Id == courseId);
    }

    public async Task<Course?> GetCourseByLessonIdAsync(Guid lessonId)
    {
        return await _db.Courses.AsNoTracking().Include(c => c.Lessons).SingleOrDefaultAsync(course => course.Lessons!.Any(l => l.Id == lessonId));
    }

    public async Task<bool> IsCourseExistsAsync(Guid courseId)
    {
        return await _db.Courses.AsNoTracking().AnyAsync(course => course.Id == courseId);
    }

    public async Task<bool> RemoveCourseAsync(Guid courseId)
    {
        Course? course = await _db.Courses.SingleOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return false;

        _db.Courses.Remove(course);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Course?> UpdateCourseAsync(Course course)
    {
        _db.Courses.Attach(course);
        _db.Entry(course).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return course;
    }
}
