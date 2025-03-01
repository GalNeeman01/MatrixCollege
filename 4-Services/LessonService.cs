using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class LessonService : IDisposable
{
    private MatrixCollegeContext _db;

    public LessonService(MatrixCollegeContext db)
    {
        _db = db;
    }

    public List<Lesson> GetAllLessons()
    {
        return _db.Lessons.AsNoTracking().ToList();
    }

    public Lesson? GetLessonById(Guid id)
    {
        return _db.Lessons.AsNoTracking().SingleOrDefault(lesson => lesson.Id == id);
    }

    public Lesson AddLesson(Lesson lesson)
    {
        _db.Lessons.Add(lesson);

        _db.SaveChanges();

        return lesson;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
