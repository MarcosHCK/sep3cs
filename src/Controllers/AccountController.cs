using DataClash.Data;
using DataClash.Services;
using DataClash.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AccountController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {   
        
        User user = null; // Reemplaza esta línea con tu lógica para buscar al usuario en la base de datos

        if (user != null)
        {
            var isPasswordValid = _authenticationService.VerifyPassword(user, user.PasswordHash, model.Password);
            if (isPasswordValid)
            {
                // La contraseña es válida, puedes iniciar sesión al usuario
                return Ok(new { Username = user.Name });
            }
        }

        // Si el usuario no existe o la contraseña no es válida, muestra un mensaje de error
        return Unauthorized();
    }
}
