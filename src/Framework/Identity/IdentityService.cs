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
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Framework.Identity
{
  public class IdentityService : IIdentityService
    {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
      private readonly IAuthorizationService _authorizationService;

      public IdentityService (
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
          _userManager = userManager;
          _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
          _authorizationService = authorizationService;
        }

      public async Task<string?> GetUserNameAsync (string userId)
        {
          var user = await _userManager.Users.FirstAsync (u => u.Id == userId);
          return user.UserName;
        }

      public async Task<(Result Result, string UserId)> CreateUserAsync (string userName, string password)
        {
          var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

          var result = await _userManager.CreateAsync (user, password);
        return (result.ToApplicationResult (), user.Id);
        }

      public async Task<bool> IsInRoleAsync (string userId, string role)
        {
          var user = _userManager.Users.SingleOrDefault (u => u.Id == userId);
          return user != null && await _userManager.IsInRoleAsync (user, role);
        }

      public async Task<bool> AuthorizeAsync (string userId, string policyName)
        {
          var user = _userManager.Users.SingleOrDefault (u => u.Id == userId);

          if (user == null)
            return false;

          var principal = await _userClaimsPrincipalFactory.CreateAsync (user);
          var result = await _authorizationService.AuthorizeAsync (principal, policyName);
        return result.Succeeded;
        }

      public async Task<Result> DeleteUserAsync (string userId)
        {
          var user = _userManager.Users.SingleOrDefault (u => u.Id == userId);
        return user != null ? await DeleteUserAsync (user) : Result.Success( );
        }

      public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
          var result = await _userManager.DeleteAsync (user);
          return result.ToApplicationResult ();
        }
    }
}
