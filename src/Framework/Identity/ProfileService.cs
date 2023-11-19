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
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DataClash.Framework.Identity
{
  public class ProfileService : IProfileService
    {
      private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
      private readonly UserManager<ApplicationUser> _userManager;

      public ProfileService (
          UserManager<ApplicationUser> userManager,
          IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
          _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
          _userManager = userManager;
        }

      public async Task GetProfileDataAsync (ProfileDataRequestContext context)
        {
          var user = await _userManager.GetUserAsync (context.Subject);
          var claims = await _userClaimsPrincipalFactory.CreateAsync (user!);
          var roles = await _userManager.GetRolesAsync (user!);
          var roleClaims = new List<Claim> ();

          foreach (string role in roles)
            {
              roleClaims.Add (new Claim (JwtClaimTypes.Role, role));
            }

          context.IssuedClaims.AddRange (roleClaims);
          context.IssuedClaims.AddRange (claims.Claims);
        }

      public async Task IsActiveAsync(IsActiveContext context)
        {
          var user = await _userManager.GetUserAsync(context.Subject);
          context.IsActive = (user != null);
        }
    }
}
