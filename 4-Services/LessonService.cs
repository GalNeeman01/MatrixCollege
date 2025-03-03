using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class LessonService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    public LessonService(MatrixCollegeContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public List<LessonDto> GetAllLessons()
    {
        List<LessonDto> dtoLessons = new List<LessonDto>();

        // Map to DTO objects
        _db.Lessons.AsNoTracking().ToList()
            .ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonDto>(lesson)));

        return dtoLessons;
    }

    public LessonDto? GetLessonById(Guid id)
    {
        Lesson? lesson = _db.Lessons.AsNoTracking().SingleOrDefault(lesson => lesson.Id == id);

        if (lesson == null) return null;
        
        return _mapper.Map<LessonDto>(lesson);
    }

    public bool IsLessonExists(Guid lessonId)
    {
        return _db.Lessons.AsNoTracking().Any(lesson => lesson.Id == lessonId);
    }

    public LessonDto AddLesson(Lesson lesson)
    {
        _db.Lessons.Add(lesson);

        _db.SaveChanges();

        // Map to DTO
        LessonDto dto = _mapper.Map<LessonDto>(lesson);

        return dto;
    }

    public List<LessonDto> GetLessonsByCourseId (Guid courseId)
    {
        List<LessonDto> dtoLessons = new List<LessonDto>();

        _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToList()
            .ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonDto>(lesson)));

        return dtoLessons;
    }

    public bool RemoveLesson(Guid lessonId)
    {
        Lesson? lesson = _db.Lessons.AsNoTracking().SingleOrDefault(lesson => lesson.Id == lessonId);

        // If no such lesson exists
        if (lesson == null) return false;

        _db.Lessons.Remove(lesson);

        _db.SaveChanges();

        return true;
    }

    public LessonDto? UpdateLesson(Lesson lesson)
    {
        if (!IsLessonExists(lesson.Id)) return null;

        _db.Lessons.Attach(lesson);
        _db.Entry(lesson).State = EntityState.Modified;
        _db.SaveChanges();

        LessonDto dto = _mapper.Map<LessonDto>(lesson);

        return dto;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
