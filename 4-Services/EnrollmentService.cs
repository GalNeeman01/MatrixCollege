using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using System.Data;

namespace Matrix;

public class EnrollmentService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    public EnrollmentService(MatrixCollegeContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<EnrollmentDto> EnrollAsync(Enrollment enrollment)
    {
        DateTime now = DateTime.Now; // Store current time

        await _db.Enrollments.AddAsync(enrollment);

        await _db.SaveChangesAsync();

        // Map to DTO
        EnrollmentDto dto = _mapper.Map<EnrollmentDto>(enrollment);

        return dto;
    }

    public async Task<List<EnrollmentDto>> GetEnrollmentsByUserIdAsync(Guid userId)
    {
        List<EnrollmentDto> dtoEnrollments = new List<EnrollmentDto>();

        List<Enrollment> dbEnrollments = await _db.Enrollments.AsNoTracking().Where(enr => enr.UserId == userId).ToListAsync();
        dbEnrollments.ForEach(enr => dtoEnrollments.Add(_mapper.Map<EnrollmentDto>(enr)));

        return dtoEnrollments;
    }

    public async Task<bool> RemoveEnrollmentAsync(Guid id)
    {
        await using IDbContextTransaction transaction = _db.Database.BeginTransaction();

        try
        {
            Enrollment? enrollment = await _db.Enrollments.SingleOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
                return false;
            
            // Retrieve enrolled course
            Course? dbCourse = await _db.Courses.SingleOrDefaultAsync(c => c.Id == enrollment.CourseId);

            if (dbCourse != null)
            {
                // Retrieve enrolled lessons
                List<Lesson> dbLessons = await _db.Lessons.Where(lesson => lesson.CourseId == enrollment.CourseId).ToListAsync();

                // Retreive progresses with matching enrollment user
                List<Progress> dbProgresses = await _db.Progresses.Where(p => p.UserId == enrollment.UserId).ToListAsync();

                // Filter progresses to only contain matches with lessons
                List<Guid> lessonsId = dbLessons.Select(l => l.Id).ToList();
                List<Progress> progresses = dbProgresses.Where(p => lessonsId.Contains(p.LessonId)).ToList();

                _db.Progresses.RemoveRange(progresses);
            }

            _db.Enrollments.Remove(enrollment);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            // Rollback and log error
            await transaction.RollbackAsync();
            Log.Error(e.Message);

            return false;
        }
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
