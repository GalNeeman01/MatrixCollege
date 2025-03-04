using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        Enrollment? enrollment = await _db.Enrollments.SingleOrDefaultAsync(e => e.Id == id);

        if (enrollment == null)
            return false;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();

        return true;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
