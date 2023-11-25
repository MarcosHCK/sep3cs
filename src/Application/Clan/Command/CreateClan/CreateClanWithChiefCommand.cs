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
using DataClash.Application.Clans.Commands.CreateClan;
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.Clans.Commands.CreateClanWithChief
{
  [Authorize]
  public record CreateClanWithChiefCommand () : CreateClanCommand;

  public class CreateClanWithChiefCommandHandler : IRequestHandler<CreateClanWithChiefCommand, long>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;

      public CreateClanWithChiefCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer)
        {
          _context = context;
          _currentPlayer = currentPlayer;
        }

      public async Task<long> Handle (CreateClanWithChiefCommand request, CancellationToken cancellationToken)
        {
          var playerIdProxy = _currentPlayer.PlayerId;
          long playerId;

          if (!_currentPlayer.PlayerId.HasValue)
            throw new ApplicationConstraintException ("User is not a player");
          else
            playerId = playerIdProxy!.Value;

          var clanEntity = new Clan
            {
              Description = request.Description,
              Name = request.Name,
              Region = request.Region,
              TotalTrophiesToEnter = request.TotalTrophiesToEnter,
              TotalTrophiesWonOnWar = request.TotalTrophiesWonOnWar,
              Type = request.Type,
            };

          clanEntity.AddDomainEvent (new ClanCreatedEvent (clanEntity));
          _context.Clans.Add (clanEntity);

          await _context.SaveChangesAsync (cancellationToken);

          var playerClanEntity = new PlayerClan
            {
              ClanId = clanEntity.Id,
              PlayerId = playerId,
              Role = ClanRole.Chief,
            };

          clanEntity.AddDomainEvent (new PlayerAddedEvent (playerClanEntity));
          _context.PlayerClans.Add (playerClanEntity);

          await _context.SaveChangesAsync (cancellationToken);
        return clanEntity.Id;
        }
    }
}
