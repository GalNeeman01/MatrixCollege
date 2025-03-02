﻿using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class UserService : IDisposable
{
    private MatrixCollegeContext _db;

    public UserService (MatrixCollegeContext matrixCollegeContext)
    {
        _db = matrixCollegeContext;
        
    }

    public User Register(User user)
    {
        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed

        _db.Users.Add(user);

        _db.SaveChanges();

        user.Password = null;

        return user;
    }

    public User? Login(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = _db.Users.AsNoTracking().SingleOrDefault(user => user.Email == credentials.Email && user.Password == credentials.Password);

        if (dbUser == null) return null;

        dbUser.Password = null; // Do not send back the user's password

        return dbUser;
    }

    public bool IsUserExists(Guid id)
    {
        return _db.Users.AsNoTracking().Any(user => user.Id == id);
    }

    public bool IsEmailUnique (string email)
    {
        return !_db.Users.AsNoTracking().Any(user => user.Email == email.ToLower());
    }

    public List<Progress> GetProgress(Guid userId)
    {
        return _db.Progresses.AsNoTracking().Where(progress => progress.UserId == userId).ToList();
    }

    public Progress AddProgress(Progress progress)
    {
        DateTime now = DateTime.Now;
        progress.WatchedAt = now;

        _db.Progresses.Add(progress);

        _db.SaveChanges();

        return progress;
    }

    public Enrollment Enroll(Enrollment enrollment)
    {
        DateTime now = DateTime.Now; // Store current time
        Enrollment dbEnrollment = new Enrollment { UserId = enrollment.UserId, CourseId = enrollment.CourseId, EnrolledAt = now }; // Create enrollment

        _db.Enrollments.Add(dbEnrollment);

        _db.SaveChanges();

        return dbEnrollment;
    }

    public Enrollment? GetEnrollmentById(Guid enrollmentId)
    {
        return _db.Enrollments.AsNoTracking().SingleOrDefault(enr => enr.Id == enrollmentId);
    }

    public List<Enrollment> GetEnrollmentsByUserId(Guid userId)
    {
        return _db.Enrollments.AsNoTracking().Where(enr => enr.UserId == userId).ToList();
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _db.Dispose();
    }
}
