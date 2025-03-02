using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Matrix;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase, IDisposable
{
    private UserService _userService;
    public IValidator<User> _validator;

    public UserController(UserService userService, IValidator<User> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    [HttpPost("/api/register")]
    public IActionResult Register([FromBody] User user)
    {
        ValidationResult validationResult = _validator.Validate(user);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationError(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

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
