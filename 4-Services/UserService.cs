using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class UserService : IDisposable
{
    private MatrixCollegeContext _db;
    private IMapper _mapper;

    public UserService (MatrixCollegeContext matrixCollegeContext, IMapper mapper)
    {
        _db = matrixCollegeContext;
        _mapper = mapper;
        
    }

    public UserResponseDto Register(User user)
    {
        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed

        _db.Users.Add(user);

        _db.SaveChanges();

        // Map to UserResponseDto
        UserResponseDto dto = _mapper.Map<UserResponseDto>(user);

        return dto;
    }

    public UserResponseDto? Login(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = _db.Users.AsNoTracking().SingleOrDefault(user => user.Email == credentials.Email && user.Password == credentials.Password);

        if (dbUser == null) return null;

        // Map to UserResponseDto
        UserResponseDto dto = _mapper.Map<UserResponseDto>(dbUser);

        return dto;
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
