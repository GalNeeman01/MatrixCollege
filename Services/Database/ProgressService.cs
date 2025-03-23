using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class ProgressService : IProgressService
{
    // DI's
    private IMapper _mapper;
    private IProgressDao _progressDao;
    private IValidationService _validationService;

    // Constructor
    public ProgressService(IMapper mapper, IProgressDao progressDao, IValidationService validationService)
    {
        _mapper = mapper;
        _progressDao = progressDao;
        _validationService = validationService;
    }

    // Methods
    public async Task<List<ProgressDto>?> GetUserProgressDtoAsync(Guid userId)
    {
        if (!(await _validationService.IsUserExistsAsync(userId)))
            return null;

        List<ProgressDto> dtoProgresses = new List<ProgressDto>();

        // Map to DTO objects
        List<Progress> dbProgresses = await _progressDao.GetUserProgressAsync(userId);
        dbProgresses.ForEach(progress => dtoProgresses.Add(_mapper.Map<ProgressDto>(progress)));

        return dtoProgresses;
    }

    public async Task<ProgressDto?> AddProgressAsync(ProgressDto progressDto)
    {
        // Return null if no matching lesson or user
        if (!(await _validationService.IsUserExistsAsync(progressDto.UserId)))
            return null;

        if (!(await _validationService.IsLessonExistsAsync(progressDto.LessonId)))
            return null;

        Progress progress = _mapper.Map<Progress>(progressDto);

        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        await _progressDao.AddProgressAsync(progress);

        // Map to DTO
        ProgressDto result = _mapper.Map<ProgressDto>(progress);

        return result;
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
