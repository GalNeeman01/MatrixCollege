namespace Matrix;

public interface ILessonService
{
    public Task<List<LessonDto>> GetAllLessonsDtoAsync();

    public Task<LessonDto?> GetLessonDtoByIdAsync(Guid id);

    public Task<List<LessonDto>?> AddLessonsAsync(List<LessonDto> lessons);

    public Task<List<LessonDto>?> GetLessonsDtoByCourseIdAsync(Guid courseId);

    public Task<List<Lesson>> GetLessonsByCourseIdAsync(Guid courseId);

    public Task<List<LessonInfoDto>?> GetLessonsInfoByCourseIdAsync(Guid courseId);

    public Task<bool> RemoveLessonsAsync(List<Guid> lessonIds);

    public Task<bool> RemoveLessonsByCourseId(Guid courseId);

    public Task<List<LessonDto>?> UpdateLessonsAsync(List<LessonDto> lessons);
}
