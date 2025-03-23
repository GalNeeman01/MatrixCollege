namespace Matrix;

public interface IUserDao
{
    public Task<User> AddUserAsync(User user);

    public Task<User?> GetUserAsync(Credentials credentials);

    public Task<bool> IsUserExistsAsync(Guid id);

    public Task<bool> IsEmailExistsAsync(string email);

    public Task<Role> GetUserRoleAsync(User user);
}
