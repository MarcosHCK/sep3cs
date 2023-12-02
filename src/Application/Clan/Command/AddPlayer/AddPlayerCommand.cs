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
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Clans.Commands.AddPlayer
{
  [Authorize]
  public record AddPlayerCommand : IRequest
    {
      public long ClanId { get; init; }
      public long PlayerId { get; init; }
      public ClanRole Role { get; init; }
    }

  public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly ICurrentUserService _currentUser;
      private readonly IIdentityService _identityService;

      private async Task<bool> CurrentPlayerIsChief (long ClanId, CancellationToken cancellationToken)
        {
          var key = new object[] { ClanId, _currentPlayer.PlayerId! };
          var playerClan = await _context.PlayerClans.FindAsync (key, cancellationToken);
        return playerClan != null && playerClan.Role == ClanRole.Chief;
        }

      public AddPlayerCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, ICurrentUserService currentUser, IIdentityService identityService)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identityService = identityService;
        }

      public async Task Handle (AddPlayerCommand request, CancellationToken cancellationToken)
        {
          if (await _identityService.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator) == false
           && await CurrentPlayerIsChief (request.ClanId, cancellationToken) == false)
            throw new ForbiddenAccessException ();
          else
            {
              var clan = await _context.Clans.FindAsync (new object[] { request.ClanId }, cancellationToken)
                          ?? throw new NotFoundException (nameof (Clan), request.ClanId);

              if (request.Role == ClanRole.Chief
               && await _context.PlayerClans.Where (e => e.ClanId == clan.Id && e.Role == ClanRole.Chief).AnyAsync (cancellationToken))
                throw new ApplicationConstraintException ("Clans has only one chief");
              else
                {
                  var playerClan = new PlayerClan { ClanId = request.ClanId, PlayerId = request.PlayerId, Role = request.Role, };

                  clan.AddDomainEvent (new PlayerAddedEvent<PlayerClan> (playerClan));
                  _context.PlayerClans.Add (playerClan);
                  await _context.SaveChangesAsync (cancellationToken);
                }
            }
        }
    }
}
