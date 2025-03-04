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

    public async Task<string> RegisterAsync(User user)
    {
        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed
        user.RoleId = (int)RolesEnum.Student;

        await _db.Users.AddAsync(user);

        await _db.SaveChangesAsync();

        user.Role = await _db.Roles.SingleAsync(role => role.Id == user.RoleId);

        return JwtHelper.GetNewToken(user);
    }

    public async Task<string?> LoginAsync(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = await _db.Users.AsNoTracking().Include(u => u.Role).SingleOrDefaultAsync(user => user.Email == credentials.Email && user.Password == credentials.Password);

        if (dbUser == null) return null;

        return JwtHelper.GetNewToken(dbUser);
    }

    public bool IsUserExists(Guid id)
    {
        return _db.Users.AsNoTracking().Any(user => user.Id == id);
    }

    public async Task<bool> IsEmailUniqueAsync (string email)
    {
        return await _db.Users.AsNoTracking().AnyAsync(user => user.Email == email.ToLower()) == false;
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _db.Dispose();
    }
}
