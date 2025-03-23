namespace Matrix;

public interface IProgressDao
{
    public Task<List<Progress>> GetUserProgressAsync(Guid userId);

    public Task<Progress> AddProgressAsync(Progress progress);

    public Task RemoveProgressesAsync(List<Progress> progresses);

    public Task<List<Progress>> GetProgressesByLesson(Guid lessonId);
}
