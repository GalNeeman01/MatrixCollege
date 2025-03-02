using Microsoft.EntityFrameworkCore;

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
