using AutoMapper;
using Matrix.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Matrix;

public class LessonService
{
    // DI's
    private MatrixCollegeContext _db;
    private ProgressService _progressService;
    private IMapper _mapper;

    // Constructor
    public LessonService(MatrixCollegeContext db, IMapper mapper, ProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
        _mapper = mapper;
    }

    // Methods
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

    public async Task<List<LessonDto>> AddLessonsAsync(List<Lesson> lessons)
    {
        await _db.Lessons.AddRangeAsync(lessons);

        await _db.SaveChangesAsync();

        // Map to DTO
        List<LessonDto> lessonDtos = lessons.Select(lesson => _mapper.Map<LessonDto>(lesson)).ToList();

        return lessonDtos;
    }

    public async Task<List<LessonDto>> GetLessonsByCourseIdAsync (Guid courseId)
    {
        List<LessonDto> dtoLessons = new List<LessonDto>();

        List<Lesson> dbLessons = await _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToListAsync();
        dbLessons.ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonDto>(lesson)));

        return dtoLessons;
    }

    public async Task<List<LessonInfoDto>> GetLessonsInfoByCourseIdAsync(Guid courseId)
    {
        List<LessonInfoDto> dtoLessons = new List<LessonInfoDto>();

        List<Lesson> dbLessons = await _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToListAsync();
        dbLessons.ForEach(lesson => dtoLessons.Add(_mapper.Map<LessonInfoDto>(lesson)));

        return dtoLessons;
    }

    public async Task<bool> RemoveLessonsAsync(List<Guid> lessonIds)
    {
        List<Lesson> lessons = await _db.Lessons.AsNoTracking().Where(lesson => lessonIds.Contains(lesson.Id)).ToListAsync();

        if (lessons.Count == 0)
            return false;

        _db.Lessons.RemoveRange(lessons);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveLessonsByCourseId(Guid courseId)
    {
        List<Lesson> lessons = await _db.Lessons.AsNoTracking().Where(lesson => lesson.CourseId == courseId).ToListAsync();

        if (lessons.Count == 0)
            return false;

        // Remove related progresses
        await _progressService.RemoveProgressByLessonsAsync(lessons);

        _db.Lessons.RemoveRange(lessons);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<LessonDto>> UpdateLessonsAsync(List<Lesson> lessons)
    {
        List<LessonDto> result = new List<LessonDto>();

        foreach (Lesson lesson in lessons)
        {
            _db.Lessons.Attach(lesson);
            _db.Entry(lesson).State = EntityState.Modified;

            // Map to Dto
            LessonDto dto = _mapper.Map<LessonDto>(lesson);
            result.Add(dto);
        }

        // Commit transaction
        await _db.SaveChangesAsync();
        return result;
    }
}
