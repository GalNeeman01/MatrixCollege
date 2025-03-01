using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase, IDisposable
{
    private UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("/api/register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_userService.IsEmailUnique(user.Email))
            return BadRequest(new ValidationError("A user with this email address is already registered."));

        User dbUser = _userService.Register(user);
        return Created("/", dbUser);
    }

    [HttpPost("/api/login")]
    public IActionResult Login([FromBody] Credentials credentials)
    {
        User? dbUser = _userService.Login(credentials);

        if (dbUser == null)
            return BadRequest(new ValidationError("Incorrect email or password."));

        return Ok(dbUser);
    }

    // Dispose of unused resources
    public void Dispose()
    {
        _userService.Dispose();
    }
}
