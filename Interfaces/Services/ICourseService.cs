namespace Matrix;

public interface ICourseService
{
    public Task<CourseDto> CreateCourseAsync(CourseDto course);

    public Task<List<CourseDto>> GetAllCoursesAsync();

    public Task<CourseDto?> GetCourseByIdAsync(Guid courseId);

    public Task<CourseDto?> GetCourseByLessonIdAsync(Guid lessonId);

    public Task<bool> IsCourseExistsAsync(Guid courseId);

    public Task<bool> RemoveCourseAsync(Guid courseId);

    public Task<CourseDto?> UpdateCourseAsync(CourseDto course);
}
