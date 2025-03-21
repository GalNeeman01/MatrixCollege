﻿namespace Matrix;

public interface ILessonService
{
    public Task<List<LessonDto>> GetAllLessonsAsync();

    public Task<LessonDto?> GetLessonByIdAsync(Guid id);

    public bool IsLessonExists(Guid lessonId);

    public Task<List<LessonDto>> AddLessonsAsync(List<Lesson> lessons);

    public Task<List<LessonDto>> GetLessonsByCourseIdAsync(Guid courseId);

    public Task<List<LessonInfoDto>> GetLessonsInfoByCourseIdAsync(Guid courseId);

    public Task<bool> RemoveLessonsAsync(List<Guid> lessonIds);

    public Task<bool> RemoveLessonsByCourseId(Guid courseId);

    public Task<List<LessonDto>> UpdateLessonsAsync(List<Lesson> lessons);
}
