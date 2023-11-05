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
                
        User user1 = (from user in Connection.default_connection.Players where user.Name == model.Username select user).First(); // Reemplaza esta línea con tu lógica para buscar al usuario en la base de datos
        if (user1==null){
            user1= (from user in Connection.default_connection.Administrators where user.Name == model.Username select user).First();
        }
        if (user1 != null)
        {
            var isPasswordValid = _authenticationService.VerifyPassword(user1, user1.PasswordHash, model.Password);
            if (isPasswordValid)
            {
                // La contraseña es válida, puedes iniciar sesión al usuario
                return Ok(new { Username = user1.Name });
            }
        }

        // Si el usuario no existe o la contraseña no es válida, muestra un mensaje de error
        return Unauthorized();
    }
}
