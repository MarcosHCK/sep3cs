/* Copyright (c) 2023-2025
 * This file is part of sep3cs.
 *
 * sep3cs is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * sep3cs is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with sep3cs. If not, see <http://www.gnu.org/licenses/>.
 */
using DataClash.Data;
using DataClash.Services;
using DataClash.Models;
using Microsoft.AspNetCore.Mvc;

[Route ("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{   
    private readonly AuthenticationService authenticationService;

    public AccountController (AuthenticationService authenticationService)
			{
				this.authenticationService = authenticationService;
			}

    [HttpPost ("login")]
    public IActionResult Login ([FromBody] LoginModel model)
			{
				User? user;

				if (model.isAdministrator)
					user = (from users in Connection.defaultConnection!.Administrators where users.Name == model.Username select users).First (null!);
				else
					user = (from users in Connection.defaultConnection!.Players where users.Name == model.Username select users).First (null!);

				if (user != null)
					{
						var isPasswordValid = authenticationService.VerifyPassword (user, user.PasswordHash, model.Password);

						if (isPasswordValid)
							{
								// La contraseña es válida, puedes iniciar sesión al usuario
								return Ok (new { Username = user.Name });
							}
					}
				return Unauthorized ();
			}
}
