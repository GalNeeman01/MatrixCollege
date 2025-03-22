
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class LessonDao : ILessonDao
{
    private MatrixCollegeContext _db;

    public LessonDao(MatrixCollegeContext db)
    {
        _db = db;
    }

    public async Task<List<Lesson>> AddLessonsAsync(List<Lesson> lessons)
    {
        await _db.Lessons.AddRangeAsync(lessons);
        await _db.SaveChangesAsync();

        return lessons;
    }

    public async Task<List<Lesson>> GetAllLessonsAsync()
    {
        return await _db.Lessons.AsNoTracking().ToListAsync();
    }

    public async Task<Lesson?> GetLessonByIdAsync(Guid id)
    {
        return await _db.Lessons.AsNoTracking().SingleOrDefaultAsync(lesson => lesson.Id == id);
    }

    public async Task<List<Lesson>> GetLessonsByCourseIdAsync(Guid courseId)
    {
        return await _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToListAsync();
    }

    public async Task<bool> IsLessonExists(Guid lessonId)
    {
        return await _db.Lessons.AsNoTracking().AnyAsync(lesson => lesson.Id == lessonId);
    }

    public async Task RemoveLessonsAsync(List<Lesson> lessons)
    {
        _db.Lessons.RemoveRange(lessons);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Lesson>> UpdateLessonsAsync(List<Lesson> lessons)
    {
        foreach (Lesson lesson in lessons)
        {
            _db.Lessons.Attach(lesson);
            _db.Entry(lesson).State = EntityState.Modified;

            await _db.SaveChangesAsync();
        }

        return lessons;
    }

    public async Task<List<Lesson>> GetLessonsByList(List<Guid> lessonIds)
    {
        return await _db.Lessons.AsNoTracking().Where(lesson => lessonIds.Contains(lesson.Id)).ToListAsync();
    }

    public async Task<bool> IsLessonsValidCourses(List<Lesson> lessons)
    {
        // Verify all lessons have valid courseIds
        foreach (Lesson lesson in lessons)
        {
            // Return false if any of the lesson courseid's do not match an existing course
            if (!(await _db.Courses.AnyAsync(c => c.Id == lesson.CourseId)))
                return false;
        }

        return true;
    }
}
