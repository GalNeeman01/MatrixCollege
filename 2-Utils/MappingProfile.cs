using AutoMapper;

namespace Matrix;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<CourseDto, Course>();

        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<EnrollmentDto, Enrollment>();

        CreateMap<Lesson, LessonDto>();
        CreateMap<LessonDto, Lesson>();
    }
}
