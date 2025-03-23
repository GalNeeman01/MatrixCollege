using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Matrix;

public class CourseService : ICourseService
{
    // DI's
    private MatrixCollegeContext _db;
    private ILessonService _lessonService;
    private IEnrollmentService _enrollmentService;
    private IValidationService _validationService;
    private IMapper _mapper;
    private ICourseDao _courseDao;

    // Constructor
    public CourseService(MatrixCollegeContext db, IMapper mapper, ILessonService lessonService, 
                        IEnrollmentService enrollmentService, ICourseDao courseDao, IValidationService validationService)
    {
        _db = db;
        _enrollmentService = enrollmentService;
        _lessonService = lessonService;
        _mapper = mapper;
        _courseDao = courseDao;
        _validationService = validationService;
    }

    // Methods
    public async Task<CourseDto> CreateCourseAsync(CourseDto courseDto)
    {
        courseDto.CreatedAt = DateTime.Now; // Set the current time

        // Map Dto to Course object
        Course course = _mapper.Map<Course>(courseDto);

        // Call DB with DAO
        Course dbCourse = await _courseDao.CreateCourseAsync(course);

        // Return DB object
        CourseDto result = _mapper.Map<CourseDto>(dbCourse);

        return result;
    }

    public async Task<List<CourseDto>> GetAllCoursesDtoAsync()
    {
        // Retrieve data from DB
        List<Course> dbCourses = await _courseDao.GetAllCoursesAsync();

        // Dtos to return
        List<CourseDto> courses = new List<CourseDto>();

        // Convert to DTO
        dbCourses.ForEach(course =>
        {
            courses.Add(_mapper.Map<CourseDto>(course));
        });

        return courses;
    }

    public async Task<CourseDto?> GetCourseDtoByIdAsync(Guid courseId)
    {
        // Retreive course from DB
        Course? dbCourse = await _courseDao.GetCourseByIdAsync(courseId);

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    public async Task<CourseDto?> GetCourseDtoByLessonIdAsync(Guid lessonId)
    {
        Course? dbCourse = await _courseDao.GetCourseByLessonIdAsync(lessonId);

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    public async Task<bool> RemoveCourseAsync(Guid courseId)
    {
        using IDbContextTransaction transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            // Return false if the course does not exist
            if (!(await _validationService.IsCourseExistsAsync(courseId)))
                return false;

            // Remove related lessons
            await _lessonService.RemoveLessonsByCourseId(courseId);

            // Remove related enrollments
            await _enrollmentService.RemoveEnrollmentsByCourseAsync(courseId);

            // Remove course
            await _courseDao.RemoveCourseAsync(courseId);

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            // Rollback transaction and send exception to catch-all filter
            await transaction.RollbackAsync();
            throw e;
        }
    }

    public async Task<CourseDto?> UpdateCourseAsync(CourseDto courseDto)
    {
        // Map to Course object
        Course course = _mapper.Map<Course>(courseDto);

        if (!(await _validationService.IsCourseExistsAsync(course.Id)))
            return null;

        // Apply changes in DB
        await _courseDao.UpdateCourseAsync(course);

        CourseDto dto = _mapper.Map<CourseDto>(course);

        return dto;
    }
}
