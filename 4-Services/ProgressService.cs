using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressService : IDisposable
{
    private MatrixCollegeContext _db;

    public ProgressService(MatrixCollegeContext db)
    {
        _db = db;
    }

    public List<Progress> GetUserProgress(Guid userId)
    {
        return _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToList();
    }

    public Progress AddProgress(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        _db.Progresses.Add(progress);

        _db.SaveChanges();

        return progress;
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _db.Dispose();
    }
}
