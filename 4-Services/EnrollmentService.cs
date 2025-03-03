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

    public EnrollmentDto Enroll(Enrollment enrollment)
    {
        DateTime now = DateTime.Now; // Store current time

        _db.Enrollments.Add(enrollment);

        _db.SaveChanges();

        // Map to DTO
        EnrollmentDto dto = _mapper.Map<EnrollmentDto>(enrollment);

        return dto;
    }

    public List<EnrollmentDto> GetEnrollmentsByUserId(Guid userId)
    {
        List<EnrollmentDto> dtoEnrollments = new List<EnrollmentDto>();

        _db.Enrollments.AsNoTracking().Where(enr => enr.UserId == userId).ToList()
            .ForEach(enr => dtoEnrollments.Add(_mapper.Map<EnrollmentDto>(enr)));

        return dtoEnrollments;
    }

    public bool RemoveEnrollment(Guid id)
    {
        Enrollment? enrollment = _db.Enrollments.SingleOrDefault(e => e.Id == id);

        if (enrollment == null)
            return false;

        _db.Enrollments.Remove(enrollment);
        _db.SaveChanges();

        return true;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
