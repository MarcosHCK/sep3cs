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

namespace DataClash.Application.PlayerCards.Commands.DeletePlayerCard
{
  [Authorize]
  public record DeletePlayerCardCommand : IRequest
    {
      public long CardId { get; init; }
      public long PlayerId { get; init; }
    }
    
  public class DeletePlayerCardCommandHandler : IRequestHandler<DeletePlayerCardCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly ICurrentUserService _currentUser;
      private readonly IIdentityService _identity;

      public DeletePlayerCardCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, ICurrentUserService currentUser, IIdentityService identity)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identity = identity;
        }
  
      public async Task Handle (DeletePlayerCardCommand request, CancellationToken cancellationToken)
        {
          if (_currentPlayer.PlayerId != request.PlayerId
            && await _identity.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator) == false)
            throw new ForbiddenAccessException ();
          else
            {
              var playerCard = await _context.PlayerCards
                        .Include (e => e.Card)
                        .Include (e => e.Player)
                        .Where (e => e.CardId == request.CardId && e.PlayerId == request.PlayerId)
                        .SingleOrDefaultAsync (cancellationToken)
                ?? throw new NotFoundException (nameof (PlayerCard), new object[] { request.CardId, request.PlayerId });

              _context.PlayerCards.Remove (playerCard);
              playerCard.Player!.AddDomainEvent (new PlayerCardDeletedEvent (playerCard));
              await _context.SaveChangesAsync (cancellationToken);
            }
        }
    }
}
