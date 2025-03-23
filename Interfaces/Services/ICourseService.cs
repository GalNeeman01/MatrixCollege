namespace Matrix;

public interface ICourseService
{
    public Task<CourseDto> CreateCourseAsync(CourseDto course);

    public Task<List<CourseDto>> GetAllCoursesDtoAsync();

    public Task<CourseDto?> GetCourseDtoByIdAsync(Guid courseId);

    public Task<CourseDto?> GetCourseDtoByLessonIdAsync(Guid lessonId);

    public Task<bool> RemoveCourseAsync(Guid courseId);

    public Task<CourseDto?> UpdateCourseAsync(CourseDto course);
}
