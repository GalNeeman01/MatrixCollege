using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressService : IProgressService
{
    // DI's
    private IMapper _mapper;
    private IProgressDao _progressDao;

    // Constructor
    public ProgressService(IMapper mapper, IProgressDao progressDao)
    {
        _mapper = mapper;
        _progressDao = progressDao;
    }

    // Methods
    public async Task<List<ProgressDto>> GetUserProgressDtoAsync(Guid userId)
    {
        List<ProgressDto> dtoProgresses = new List<ProgressDto>();

        // Map to DTO objects
        List<Progress> dbProgresses = await _progressDao.GetUserProgressAsync(userId);
        dbProgresses.ForEach(progress => dtoProgresses.Add(_mapper.Map<ProgressDto>(progress)));

        return dtoProgresses;
    }

    public async Task<ProgressDto> AddProgressAsync(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        await _progressDao.AddProgressAsync(progress);

        // Map to DTO
        ProgressDto dto = _mapper.Map<ProgressDto>(progress);

        return dto;
    }

    public async Task RemoveProgressByLessonsAsync(List<Lesson> lessons)
    {
        List<Progress> progressesToDelete = new List<Progress>();

        foreach (Lesson lesson in lessons)
            progressesToDelete.AddRange(await _progressDao.GetProgressesByLesson(lesson.Id));

        await _progressDao.RemoveProgressesAsync(progressesToDelete);
    }

    public async Task RemoveProgressesAsync(List<Progress> progresses)
    {
        await _progressDao.RemoveProgressesAsync(progresses);
    }

    public async Task<List<Progress>> GetUserProgressAsync(Guid userId)
    {
        return await _progressDao.GetUserProgressAsync(userId); ;
    }
}
