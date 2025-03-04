using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class CourseService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;


    public CourseService(MatrixCollegeContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CourseDto> CreateCourse(Course course)
    {
        course.CreatedAt = DateTime.Now; // Set the current time

        await _db.Courses.AddAsync(course);

        await _db.SaveChangesAsync();

        CourseDto courseDto = _mapper.Map<CourseDto>(course);

        return courseDto;
    }

    public async Task<List<CourseDto>> GetAllCourses()
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

    public async Task<CourseDto?> GetCourseById(Guid courseId)
    {
        Course? dbCourse = await _db.Courses.AsNoTracking().SingleOrDefaultAsync(course => course.Id == courseId);

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    // Return whether a course exists in the DB
    public bool IsCourseExists(Guid courseId)
    {
        return _db.Courses.AsNoTracking().Any(course => course.Id == courseId);
    }

    public async Task<bool> RemoveCourse(Guid courseId)
    {
        Course? course = await _db.Courses.AsNoTracking().SingleOrDefaultAsync(course => course.Id == courseId);

        // If no such course exists
        if (course == null) return false;

        _db.Courses.Remove(course);

        await _db.SaveChangesAsync();

        return true;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
