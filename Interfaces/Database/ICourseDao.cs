namespace Matrix;

public interface ICourseDao
{
    public Task<Course> CreateCourseAsync(Course course);

    public Task<List<Course>> GetAllCoursesAsync();

    public Task<Course?> GetCourseByIdAsync(Guid courseId);

    public Task<Course?> GetCourseByLessonIdAsync(Guid lessonId);

    public Task<bool> IsCourseExistsAsync(Guid courseId);

    public Task<bool> RemoveCourseAsync(Guid courseId);

    public Task<Course?> UpdateCourseAsync(Course course);
}
