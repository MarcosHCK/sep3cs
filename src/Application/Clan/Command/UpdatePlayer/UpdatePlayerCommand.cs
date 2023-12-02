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
using DataClash.Application.Clans.Commands.AddPlayer;
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using MediatR;

namespace DataClash.Application.Clans.Commands.UpdatePlayer
{
  public record UpdatePlayerCommand : AddPlayerCommand;

  public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand>
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

      public UpdatePlayerCommandHandler (
          IApplicationDbContext context,
          ICurrentPlayerService currentPlayer,
          ICurrentUserService currentUser,
          IIdentityService identityService)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identityService = identityService;
        }

      public async Task Handle (UpdatePlayerCommand command, CancellationToken cancellationToken)
        {
          if (await _identityService.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator) == false
            && await CurrentPlayerIsChief (command.ClanId, cancellationToken) == false)
            throw new ForbiddenAccessException ();
          else if (_currentPlayer.PlayerId == command.PlayerId)
            throw new ApplicationConstraintException ("Clan chief can not be changed");
          else
            {
              var clan = await _context.Clans.FindAsync (new object[] { command.ClanId }, cancellationToken)
                        ?? throw new NotFoundException (nameof (Clan), command.ClanId);
              var playerClan = await _context.PlayerClans.FindAsync (new object[] { clan.Id, command.PlayerId }, cancellationToken)
                          ?? throw new NotFoundException (nameof (WarClan), new object[] { clan.Id, command.PlayerId });

              playerClan.Role = command.Role;
              await _context.SaveChangesAsync (cancellationToken);
            }
        }
    }
}
