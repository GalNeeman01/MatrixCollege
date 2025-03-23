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
    private ITokenService _tokenService;
    private IMapper _mapper;
    private IUserDao _userDao;

    // Constructor
    public UserService (IMapper mapper, ITokenService tokenService,
                        IUserDao userDao)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _userDao = userDao;
    }

    // Methods
    public async Task<string?> RegisterAsync(CreateUserDto userDto)
    {
        if (await _userDao.IsEmailExistsAsync(userDto.Email))
            return null;

        // Map to User object
        User user = _mapper.Map<User>(userDto);

        user.Email = user.Email.ToLower(); // Format email
        user.Password = Encryptor.GetHashed(user.Password); // Convert to hashed
        user.RoleId = (user.RoleId == 2 || user.RoleId == 3) ? user.RoleId : (int)RolesEnum.Student;

        await _userDao.AddUserAsync(user);

        user.Role = await _userDao.GetUserRoleAsync(user);

        return _tokenService.GetNewToken(user);
    }

    public async Task<string?> LoginAsync(Credentials credentials)
    {
        credentials.Email = credentials.Email.ToLower(); // Format email
        credentials.Password = Encryptor.GetHashed(credentials.Password); // Convert to hashed

        // Retrieve user from DB 
        User? dbUser = await _userDao.GetUserAsync(credentials);

        if (dbUser == null) return null;

        return _tokenService.GetNewToken(dbUser);
    }

    public async Task<bool> IsEmailUniqueAsync (string email)
    {
        return !(await _userDao.IsEmailExistsAsync(email)); // Unique = not exists 👍
    }
}
