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

    public async Task<List<ProgressDto>> GetUserProgress(Guid userId)
    {
        List<ProgressDto> dtoProgresses = new List<ProgressDto>();

        // Map to DTO objects
        List<Progress> dbProgresses = await _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToListAsync();
        dbProgresses.ForEach(progress => dtoProgresses.Add(_mapper.Map<ProgressDto>(progress)));

        return dtoProgresses;
    }

    public async Task<ProgressDto> AddProgress(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        await _db.Progresses.AddAsync(progress);

        await _db.SaveChangesAsync();

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
