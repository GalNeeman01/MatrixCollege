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

    public void Dispose()
    {
        _db.Dispose();
    }
}
