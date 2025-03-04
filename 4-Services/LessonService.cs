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

    public async Task<List<LessonDto>> GetAllLessonsAsync()
    {
        List<LessonDto> dtoLessons = new List<LessonDto>();

        // Map to DTO objects
        List<Lesson> dbLessons = await _db.Lessons.AsNoTracking().ToListAsync();
        dbLessons.ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonDto>(lesson)));

        return dtoLessons;
    }

    public async Task<LessonDto?> GetLessonByIdAsync(Guid id)
    {
        Lesson? lesson = await _db.Lessons.AsNoTracking().SingleOrDefaultAsync(lesson => lesson.Id == id);

        if (lesson == null) return null;
        
        return _mapper.Map<LessonDto>(lesson);
    }

    public bool IsLessonExists(Guid lessonId)
    {
        return _db.Lessons.AsNoTracking().Any(lesson => lesson.Id == lessonId);
    }

    public async Task<LessonDto> AddLessonAsync(Lesson lesson)
    {
        await _db.Lessons.AddAsync(lesson);

        await _db.SaveChangesAsync();

        // Map to DTO
        LessonDto dto = _mapper.Map<LessonDto>(lesson);

        return dto;
    }

    public async Task<List<LessonDto>> GetLessonsByCourseIdAsync (Guid courseId)
    {
        List<LessonDto> dtoLessons = new List<LessonDto>();

        List<Lesson> dbLessons = await _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToListAsync();
        dbLessons.ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonDto>(lesson)));

        return dtoLessons;
    }

    public async Task<bool> RemoveLessonAsync(Guid lessonId)
    {
        Lesson? lesson = await _db.Lessons.AsNoTracking().SingleOrDefaultAsync(lesson => lesson.Id == lessonId);

        // If no such lesson exists
        if (lesson == null) return false;

        _db.Lessons.Remove(lesson);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<LessonDto?> UpdateLessonAsync(Lesson lesson)
    {
        if (!IsLessonExists(lesson.Id)) return null;

        _db.Lessons.Attach(lesson);
        _db.Entry(lesson).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        LessonDto dto = _mapper.Map<LessonDto>(lesson);

        return dto;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
