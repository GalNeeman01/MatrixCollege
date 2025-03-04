using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public enum RolesEnum
{
    Admin = 1,
    Student = 2,
    Professor = 3
};

public class UserService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    public UserService (MatrixCollegeContext matrixCollegeContext, IMapper mapper)
    {
        _db = matrixCollegeContext;
        _mapper = mapper;
        
    }

    public string Register(User user)
    {
        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed
        user.RoleId = (int)RolesEnum.Student;

        _db.Users.Add(user);

        _db.SaveChanges();

        user.Role = _db.Roles.Single(role => role.Id == user.RoleId);

        return JwtHelper.GetNewToken(user);
    }

    public string? Login(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = _db.Users.AsNoTracking().Include(u => u.Role).SingleOrDefault(user => user.Email == credentials.Email && user.Password == credentials.Password);

        if (dbUser == null) return null;

        return JwtHelper.GetNewToken(dbUser);
    }

    public bool IsUserExists(Guid id)
    {
        return _db.Users.AsNoTracking().Any(user => user.Id == id);
    }

    public bool IsEmailUnique (string email)
    {
        return !_db.Users.AsNoTracking().Any(user => user.Email == email.ToLower());
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _db.Dispose();
    }
}
