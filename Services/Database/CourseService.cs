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
    private IMapper _mapper;

    // Constructor
    public CourseService(MatrixCollegeContext db, IMapper mapper, ILessonService lessonService, IEnrollmentService enrollmentService)
    {
        _db = db;
        _enrollmentService = enrollmentService;
        _lessonService = lessonService;
        _mapper = mapper;
    }

    // Methods
    public async Task<CourseDto> CreateCourseAsync(Course course)
    {
        course.CreatedAt = DateTime.Now; // Set the current time

        await _db.Courses.AddAsync(course);

        await _db.SaveChangesAsync();

        CourseDto courseDto = _mapper.Map<CourseDto>(course);

        return courseDto;
    }

    public async Task<List<CourseDto>> GetAllCoursesAsync()
    {
        // Dtos to return
        List<CourseDto> courses = new List<CourseDto>();

        // Run through returned data and convert to DTO objects
        List<Course> dbCourses = await _db.Courses.AsNoTracking().ToListAsync();

        dbCourses.ForEach(course =>
        {
            courses.Add(_mapper.Map<CourseDto>(course));
        });

        return courses;
    }

    public async Task<CourseDto?> GetCourseByIdAsync(Guid courseId)
    {
        Course? dbCourse = await _db.Courses.AsNoTracking().SingleOrDefaultAsync(course => course.Id == courseId);

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    public async Task<CourseDto?> GetCourseByLessonIdAsync(Guid lessonId)
    {
        Course? dbCourse = await _db.Courses.AsNoTracking().Include(c => c.Lessons).SingleOrDefaultAsync(course => course.Lessons!.Any(l => l.Id == lessonId));

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    // Return whether a course exists in the DB
    public async Task<bool> IsCourseExistsAsync(Guid courseId)
    {
        return await _db.Courses.AsNoTracking().AnyAsync(course => course.Id == courseId);
    }

    public async Task<bool> RemoveCourseAsync(Guid courseId)
    {
        // Use a transaction to verify full deletion of all related items
        await using IDbContextTransaction transaction = _db.Database.BeginTransaction();

        try
        {
            Course course = await _db.Courses.AsNoTracking().SingleAsync(course => course.Id == courseId);

            // Remove related lessons
            await _lessonService.RemoveLessonsByCourseId(courseId);

            // Remove related enrollments
            await _enrollmentService.RemoveEnrollmentsByCourseAsync(courseId);

            // Remove course
            _db.Courses.Remove(course);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Log.Error(e.Message);
            return false;
        }
    }

    public async Task<CourseDto?> UpdateCourseAsync(Course course)
    {
        if (!(await IsCourseExistsAsync(course.Id)))
            return null;

        _db.Courses.Attach(course);
        _db.Entry(course).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        CourseDto dto = _mapper.Map<CourseDto>(course);

        return dto;
    }
}
