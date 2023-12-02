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

namespace DataClash.Application.Clans.Commands.LeaveWar
{
  [Authorize]
  public record LeaveWarCommand : IRequest
    {
      public long ClanId { get; init; }
      public long WarId { get; init; }
    }

  public class LeaveWarCommandHandler : IRequestHandler<LeaveWarCommand>
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

      public LeaveWarCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, ICurrentUserService currentUser, IIdentityService identityService)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identityService = identityService;
        }

      public async Task Handle (LeaveWarCommand command, CancellationToken cancellationToken)
        {
          if (await _identityService.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator) == false
            && await CurrentPlayerIsChief (command.ClanId, cancellationToken) == false)
            throw new ForbiddenAccessException ();
          else
            {
              var clan = await _context.Clans.FindAsync (new object[] { command.ClanId }, cancellationToken)
                        ?? throw new NotFoundException (nameof (Clan), command.ClanId);
              var warClan = await _context.WarClans.FindAsync (new object[] { clan.Id, command.WarId }, cancellationToken)
                        ?? throw new NotFoundException (nameof (WarClan), new object[] { clan.Id, command.WarId });

              _context.WarClans.Remove (warClan);
              clan.AddDomainEvent (new LeftWarEvent (warClan));

              await _context.SaveChangesAsync (cancellationToken);
            }
        }
    }
}
