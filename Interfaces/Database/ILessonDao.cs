namespace Matrix;

public interface ILessonDao
{
    public Task<List<Lesson>> GetAllLessonsAsync();

    public Task<Lesson?> GetLessonByIdAsync(Guid id);

    public Task<bool> IsLessonExists(Guid lessonId);

    public Task<List<Lesson>> AddLessonsAsync(List<Lesson> lessons);

    public Task<List<Lesson>> GetLessonsByCourseIdAsync(Guid courseId);

    public Task RemoveLessonsAsync(List<Lesson> lessons);

    public Task<List<Lesson>> UpdateLessonsAsync(List<Lesson> lessons);

    public Task<List<Lesson>> GetLessonsByList(List<Guid> lessonIds);

    public Task<bool> IsLessonsValidCourses(List<Lesson> lessons);
}
