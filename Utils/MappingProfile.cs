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

        CreateMap<Progress, ProgressDto>();
        CreateMap<ProgressDto, Progress>();

        CreateMap<User, CreateUserDto>();
        CreateMap<CreateUserDto, User>();

        CreateMap<User, UserResponseDto>();
        CreateMap<UserResponseDto, User>();

        CreateMap<Lesson, LessonInfoDto>();
        CreateMap<LessonInfoDto, Lesson>();
    }
}
