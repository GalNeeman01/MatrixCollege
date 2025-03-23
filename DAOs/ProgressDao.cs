using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressDao : IProgressDao
{
    private MatrixCollegeContext _db;

    public ProgressDao(MatrixCollegeContext db)
    {
        _db = db;
    }

    public async Task<Progress> AddProgressAsync(Progress progress)
    {
        await _db.Progresses.AddAsync(progress);

        await _db.SaveChangesAsync();

        return progress;
    }

    public async Task<List<Progress>> GetUserProgressAsync(Guid userId)
    {
        return await _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToListAsync();
    }

    public async Task<List<Progress>> GetProgressesByLesson(Guid lessonId)
    {
        return await _db.Progresses.Where(p => p.LessonId == lessonId).ToListAsync();
    }

    public async Task RemoveProgressesAsync(List<Progress> progresses)
    {
        _db.Progresses.RemoveRange(progresses);
        await _db.SaveChangesAsync();
    }
}
