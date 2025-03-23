namespace Matrix;

public interface IUserService
{
    public Task<string?> RegisterAsync(CreateUserDto user);

    public Task<string?> LoginAsync(Credentials credentials);

    public Task<bool> IsEmailUniqueAsync(string email);
}
