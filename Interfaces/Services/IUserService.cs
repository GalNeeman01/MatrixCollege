namespace Matrix;

public interface IUserService
{
    public Task<string> RegisterAsync(User user);

    public Task<string?> LoginAsync(Credentials credentials);

    public bool IsUserExists(Guid id);

    public Task<bool> IsEmailUniqueAsync(string email);
}
