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
using DataClash.Framework.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataClash.Framework.Persistence
{
  public class ApplicationDbContextInitialiser
    {
      private readonly ILogger<ApplicationDbContextInitialiser> _logger;
      private readonly ApplicationDbContext _context;
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly RoleManager<IdentityRole> _roleManager;

      public ApplicationDbContextInitialiser (
          ILogger<ApplicationDbContextInitialiser> logger,
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager)
        {
          _logger = logger;
          _context = context;
          _userManager = userManager;
          _roleManager = roleManager;
        }

      public async Task InitialiseAsync ()
        {
          try
            {
              if (_context.Database.IsSqlite ())
                await _context.Database.MigrateAsync ();
            }
          catch (Exception ex)
            {
              _logger.LogError (ex, "An error occurred while initialising the database.");
              throw;
            }
        }

      public async Task SeedAsync ()
        {
          try
            {
              await TrySeedAsync ();
            }
          catch (Exception ex)
            {
              _logger.LogError (ex, "An error occurred while seeding the database.");
              throw;
            }
        }

      public async Task TrySeedAsync ()
        {
          // Default roles
          var administratorRole = new IdentityRole ("Administrator");

          if (_roleManager.Roles.All (r => r.Name != administratorRole.Name))
            await _roleManager.CreateAsync (administratorRole);

          var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

          if (_userManager.Users.All (u => u.UserName != administrator.UserName))
            {
              await _userManager.CreateAsync (administrator, "Administrator1!");

              if (!string.IsNullOrWhiteSpace (administratorRole.Name))
                await _userManager.AddToRolesAsync (administrator, new [] { administratorRole.Name });
            }
        }
    }
}
