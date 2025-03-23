
namespace Matrix;

public class ValidationService : IValidationService
{
    private ICourseDao _courseDao;
    private ILessonDao _lessonDao;
    private IUserDao _userDao;
    private IEnrollmentDao _enrollmentDao;

    public ValidationService(ICourseDao courseDao, ILessonDao lessonDao, 
                            IUserDao userDao, IEnrollmentDao enrollmentDao)
    {
        _courseDao = courseDao;
        _lessonDao = lessonDao;
        _userDao = userDao;
        _enrollmentDao = enrollmentDao;
    }

    // Return whether a course exists in the DB
    public async Task<bool> IsCourseExistsAsync(Guid courseId)
    {
        return await _courseDao.IsCourseExistsAsync(courseId);
    }

    public async Task<bool> IsUserExistsAsync(Guid userId)
    {
        return await _userDao.IsUserExistsAsync(userId);
    }

    public async Task<bool> IsLessonExistsAsync(Guid lessonId)
    {
        return await _lessonDao.IsLessonExists(lessonId);
    }

    public async Task<bool> IsEnrollmentExistsAsync(Guid enrollmentId)
    {
        return await _enrollmentDao.IsEnrollmentExists(enrollmentId);
    }
}
