
using Microsoft.EntityFrameworkCore;

namespace Matrix;

public class UserDao : IUserDao
{
    private MatrixCollegeContext _db;

    public UserDao (MatrixCollegeContext db)
    {
        _db = db;
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);

        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserAsync(Credentials credentials)
    {
        return await _db.Users.AsNoTracking().Include(u => u.Role).SingleOrDefaultAsync(user => user.Email == credentials.Email && user.Password == credentials.Password);
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _db.Users.AsNoTracking().AnyAsync(user => user.Email == email.ToLower());
    }

    public async Task<bool> IsUserExistsAsync(Guid id)
    {
        return await _db.Users.AsNoTracking().AnyAsync(user => user.Id == id);
    }

    public async Task<Role> GetUserRoleAsync(User user)
    {
        return await _db.Roles.SingleAsync(r => r.Id == user.RoleId);
    }
}
