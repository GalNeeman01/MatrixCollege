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

    public CourseDto CreateCourse(Course course)
    {
        course.CreatedAt = DateTime.Now; // Set the current time

        _db.Courses.Add(course);

        _db.SaveChanges();

        CourseDto courseDto = _mapper.Map<CourseDto>(course);

        return courseDto;
    }

    public List<CourseDto> GetAllCourses()
    {
        // Dtos to return
        List<CourseDto> courses = new List<CourseDto>();

        // Run through returned data and convert to DTO objects
        _db.Courses.AsNoTracking().ToList().ForEach(course =>
        {
            courses.Add(_mapper.Map<CourseDto>(course));
        });

        return courses;
    }

    public CourseDto? GetCourseById(Guid courseId)
    {
        Course? dbCourse = _db.Courses.AsNoTracking().SingleOrDefault(course => course.Id == courseId);

        if (dbCourse == null) return null;

        return _mapper.Map<CourseDto>(dbCourse);
    }

    // Return whether a course exists in the DB
    public bool IsCourseExists(Guid courseId)
    {
        return _db.Courses.AsNoTracking().Any(course => course.Id == courseId);
    }

    public bool RemoveCourse(Guid courseId)
    {
        Course? course = _db.Courses.AsNoTracking().SingleOrDefault(course => course.Id == courseId);

        // If no such course exists
        if (course == null) return false;

        _db.Courses.Remove(course);

        _db.SaveChanges();

        return true;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
