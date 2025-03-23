using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using System.Data;

namespace Matrix;

public class EnrollmentService : IEnrollmentService
{
    // DI's
    private MatrixCollegeContext _db;
    private IMapper _mapper;
    private IEnrollmentDao _enrollmentDao;
    private IProgressService _progressService;
    private ILessonService _lessonService;

    // Constructor
    public EnrollmentService(MatrixCollegeContext db, IMapper mapper, IEnrollmentDao enrollmentDao,
                            IProgressService progressService, ILessonService lessonService)
    {
        _db = db;
        _mapper = mapper;
        _enrollmentDao = enrollmentDao;
        _progressService = progressService;
        _lessonService = lessonService;
    }

    // Methods
    public async Task<EnrollmentDto> EnrollAsync(Enrollment enrollment)
    {
        DateTime now = DateTime.Now; // Store current time

        await _enrollmentDao.AddEnrollmentAsync(enrollment);

        // Map to DTO
        EnrollmentDto dto = _mapper.Map<EnrollmentDto>(enrollment);

        return dto;
    }

    public async Task<List<EnrollmentDto>> GetEnrollmentsByUserIdAsync(Guid userId)
    {
        List<EnrollmentDto> dtoEnrollments = new List<EnrollmentDto>();

        List<Enrollment> dbEnrollments = await _enrollmentDao.GetEnrollmentsByUserIdAsync(userId);
        dbEnrollments.ForEach(enr => dtoEnrollments.Add(_mapper.Map<EnrollmentDto>(enr)));

        return dtoEnrollments;
    }

    public async Task<bool> IsEnrollmentExistsAsync(Guid enrollmentId)
    {
        return await _enrollmentDao.IsEnrollmentExists(enrollmentId);
    }

    public async Task<bool> RemoveEnrollmentAsync(Guid id)
    {
        if (!await IsEnrollmentExistsAsync(id))
            return false;

        await using IDbContextTransaction transaction = _db.Database.BeginTransaction();

        try
        {
            Enrollment enrollment = (await _enrollmentDao.GetEnrollmentByIdAsync(id))!; // Already made sure it exists so not null
            
            // Retrieve enrolled course
            Course? dbCourse = await _db.Courses.SingleOrDefaultAsync(c => c.Id == enrollment.CourseId);

            if (dbCourse != null)
            {
                // Retrieve enrolled lessons
                List<Lesson> dbLessons = await _lessonService.GetLessonsByCourseIdAsync(enrollment.CourseId);

                // Retreive progresses with matching enrollment user
                List<Progress> dbProgresses = await _progressService.GetUserProgressAsync(enrollment.UserId);

                // Filter progresses to only contain matches with lessons
                List<Guid> lessonsId = dbLessons.Select(l => l.Id).ToList();
                List<Progress> progresses = dbProgresses.Where(p => lessonsId.Contains(p.LessonId)).ToList();

                await _progressService.RemoveProgressesAsync(progresses);
            }

            await _enrollmentDao.RemoveEnrollmentAsync(enrollment.Id);

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            // Rollback and log error
            await transaction.RollbackAsync();
            throw e;
        }
    }

    public async Task RemoveEnrollmentsByCourseAsync(Guid courseId)
    {
        await using IDbContextTransaction transaction = _db.Database.BeginTransaction();

        try
        {
            List<Enrollment> enrollments = await _enrollmentDao.GetEnrollmentsByCourseId(courseId);

            foreach (Enrollment enrollment in enrollments)
                await RemoveEnrollmentAsync(enrollment.Id);

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw e;
        }
    }
}
