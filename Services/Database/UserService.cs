using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Matrix;

// Enum for roles
public enum RolesEnum
{
    Admin = 1,
    Student = 2,
    Professor = 3
};

public class UserService : IUserService
{
    // DI's
    private MatrixCollegeContext _db;
    private TokenService _tokenService;
    private IMapper _mapper;

    // Constructor
    public UserService (MatrixCollegeContext matrixCollegeContext, IMapper mapper, TokenService tokenService)
    {
        _db = matrixCollegeContext;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    // Methods
    public async Task<string> RegisterAsync(User user)
    {
        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed
        user.RoleId = (user.RoleId == 2 || user.RoleId == 3) ? user.RoleId : (int)RolesEnum.Student;

        await _db.Users.AddAsync(user);

        await _db.SaveChangesAsync();

        user.Role = await _db.Roles.SingleAsync(role => role.Id == user.RoleId);

        return _tokenService.GetNewToken(user);
    }

    public async Task<string?> LoginAsync(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = await _db.Users.AsNoTracking().Include(u => u.Role).SingleOrDefaultAsync(user => user.Email == credentials.Email && user.Password == credentials.Password);

        if (dbUser == null) return null;

        return _tokenService.GetNewToken(dbUser);
    }

    public bool IsUserExists(Guid id)
    {
        return _db.Users.AsNoTracking().Any(user => user.Id == id);
    }

    public async Task<bool> IsEmailUniqueAsync (string email)
    {
        return await _db.Users.AsNoTracking().AnyAsync(user => user.Email == email.ToLower()) == false;
    }
}
