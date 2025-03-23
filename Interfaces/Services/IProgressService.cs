namespace Matrix;

public interface IProgressService
{
    public Task<List<ProgressDto>?> GetUserProgressDtoAsync(Guid userId);

    public Task<List<Progress>> GetUserProgressAsync(Guid userId);

    public Task<ProgressDto?> AddProgressAsync(ProgressDto progress);

    public Task RemoveProgressByLessonsAsync(List<Lesson> lessons);

    public Task RemoveProgressesAsync(List<Progress> progresses);
}
