using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressService
{
    // DI's
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    // Constructor
    public ProgressService(MatrixCollegeContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // Methods
    public async Task<List<ProgressDto>> GetUserProgressAsync(Guid userId)
    {
        List<ProgressDto> dtoProgresses = new List<ProgressDto>();

        // Map to DTO objects
        List<Progress> dbProgresses = await _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToListAsync();
        dbProgresses.ForEach(progress => dtoProgresses.Add(_mapper.Map<ProgressDto>(progress)));

        return dtoProgresses;
    }

    public async Task<ProgressDto> AddProgressAsync(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        await _db.Progresses.AddAsync(progress);

        await _db.SaveChangesAsync();

        // Map to DTO
        ProgressDto dto = _mapper.Map<ProgressDto>(progress);

        return dto;
    }

    public async Task RemoveProgressByLessonsAsync(List<Lesson> lessons)
    {
        List<Progress> progressesToDelete = new List<Progress>();

        foreach (Lesson lesson in lessons)
            progressesToDelete.AddRange(await _db.Progresses.Where(p => p.LessonId == lesson.Id).ToListAsync());

        _db.Progresses.RemoveRange(progressesToDelete);
        await _db.SaveChangesAsync();
    }
}
