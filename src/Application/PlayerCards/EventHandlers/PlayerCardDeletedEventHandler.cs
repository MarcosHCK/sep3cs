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
using DataClash.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DataClash.Application.PlayerCards.EventHandlers
{
  public class PlayerCardDeletedEventHandler : INotificationHandler<PlayerCardDeletedEvent>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly ILogger<PlayerCardDeletedEventHandler> _logger;

      public PlayerCardDeletedEventHandler(IApplicationDbContext context, ICurrentPlayerService currentPlayer, ILogger<PlayerCardDeletedEventHandler> logger)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _logger = logger;
        }

      public async Task Handle(PlayerCardDeletedEvent notification, CancellationToken cancellationToken)
        {
          _logger.LogInformation("DataClash Domain Event: {DomainEvent}", notification.GetType ().Name);
          var player = await _context.Players.FindAsync (new object[] { _currentPlayer.PlayerId! }, cancellationToken);

          if (player!.FavoriteCardId == notification.Item.CardId)
            {
              player.FavoriteCardId = null;
            }
        }
    }
}
