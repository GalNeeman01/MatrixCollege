using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    public ProgressService(MatrixCollegeContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public List<ProgressDto> GetUserProgress(Guid userId)
    {
        List<ProgressDto> dtoProgresses = new List<ProgressDto>();

        // Map to DTO objects
        _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToList()
            .ForEach(progress => dtoProgresses.Add(_mapper.Map<ProgressDto>(progress)));

        return dtoProgresses;
    }

    public ProgressDto AddProgress(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        _db.Progresses.Add(progress);

        _db.SaveChanges();

        // Map to DTO
        ProgressDto dto = _mapper.Map<ProgressDto>(progress);

        return dto;
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _db.Dispose();
    }
}
