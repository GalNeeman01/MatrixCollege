﻿using FluentValidation;

namespace Matrix;

public class CourseValidator : AbstractValidator<Course>, IDisposable
{
    // DI
    private CourseService _courseService;

    public CourseValidator(CourseService courseService)
    {
        _courseService = courseService;

        RuleFor(course => course.Title).NotNull().WithMessage("Title is a required field.")
            .MinimumLength(2).WithMessage("Title must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters in length.");

        RuleFor(course => course.Description).NotNull().WithMessage("Description is a required field.")
            .MaximumLength(3000).WithMessage("Description cannot exceed 3000 characters in length.");
    }

    public void Dispose()
    {
        _courseService.Dispose();
    }
}
