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

namespace DataClash.Application.PlayerCards.Commands.CreatePlayerCard
{
  [Authorize]
  public record CreatePlayerCardCommand : IRequest
    {
      public long CardId { get; init; }
      public long PlayerId { get; init; }
      public int Level { get; init; }
    }

  public class CreatePlayerCardCommandHandler : IRequestHandler<CreatePlayerCardCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly ICurrentUserService _currentUser;
      private readonly IIdentityService _identity;

      public CreatePlayerCardCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, ICurrentUserService currentUser, IIdentityService identity)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identity = identity;
        }

      public async Task Handle (CreatePlayerCardCommand request, CancellationToken cancellationToken)
        {
          if (_currentPlayer.PlayerId != request.PlayerId
            && await _identity.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator) == false)
            throw new ForbiddenAccessException ();
          else
            {
              var playerCard = new PlayerCard
                {
                  CardId = request.CardId,
                  PlayerId = request.PlayerId,
                  Level = request.Level,
                };

              var player = await _context.Players.FindAsync (new object[] { request.PlayerId }, cancellationToken)
                ?? throw new NotFoundException (nameof (Player), new object[] { request.PlayerId });

              player.AddDomainEvent (new PlayerCardCreatedEvent (playerCard));
              await _context.PlayerCards.AddAsync (playerCard, cancellationToken);
              await _context.SaveChangesAsync (cancellationToken);
            }
        }
    }
}
