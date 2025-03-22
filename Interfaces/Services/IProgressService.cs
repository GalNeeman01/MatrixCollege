namespace Matrix;

public interface IProgressService
{
    public Task<List<ProgressDto>> GetUserProgressAsync(Guid userId);

    public Task<ProgressDto> AddProgressAsync(Progress progress);

    public Task RemoveProgressByLessonsAsync(List<Lesson> lessons);
}
