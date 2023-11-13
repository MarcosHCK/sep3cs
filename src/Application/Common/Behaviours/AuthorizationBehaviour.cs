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
using System.Reflection;
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using MediatR;

namespace DataClash.Application.Common.Behaviours
{
  public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : notnull
    {
      private readonly ICurrentUserService _currentUserService;
      private readonly IIdentityService _identityService;

      public AuthorizationBehaviour (ICurrentUserService currentUserService, IIdentityService identityService)
        {
          _currentUserService = currentUserService;
          _identityService = identityService;
        }

      private async Task<bool> AuthorizeByRoles (string userId, IEnumerable<AuthorizeAttribute> attributes)
        {
          foreach (var roles in attributes.Select (a => a.Roles.Split (',')))
          foreach (var role in roles)
            {
              if (await _identityService.IsInRoleAsync (userId, role))
                return true;
            }
        return false;
        }

      public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
          var attrs = request.GetType ().GetCustomAttributes <AuthorizeAttribute> ();

          if (attrs.Any ())
            {
              var userId = _currentUserService.UserId;

              if (userId == null)
                throw new UnauthorizedAccessException ();

              var roles = attrs.Where (a => !string.IsNullOrEmpty (a.Roles));

              if (roles.Any ())
                {
                  if (await AuthorizeByRoles (userId, roles) == false)
                    throw new ForbiddenAccessException ();
                }

              var policies = attrs.Where (a => !string.IsNullOrEmpty (a.Policy));

              if (policies.Any ())
                {
                  foreach (var policy in policies.Select (a => a.Policy))
                    {
                      if (await _identityService.AuthorizeAsync (userId, policy) == false)
                        throw new ForbiddenAccessException ();
                    }
                }
            }
        return await next ();
        }
    }
}
