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
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.Players.Commands.UpdatePlayer
{
  [Authorize]
  public record UpdatePlayerCommand : IRequest
    {
      public long Id { get; init; }
      public long? FavoriteCardId { get; init; }
      public string? Nickname { get; init; }
      public long Level { get; init; }
      public long TotalCardsFound { get; init; }
      public long TotalThrophies { get; init; }
      public long TotalWins { get; init; }
    }

  public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand>
    {
      private readonly IApplicationDbContext _context;

      public UpdatePlayerCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task Handle (UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
          var entity = await _context.Players.FindAsync (new object [] { request.Id }, cancellationToken) ?? throw new NotFoundException (nameof (Player), request.Id);

          entity.FavoriteCardId = request.FavoriteCardId;
          entity.Level = request.Level;
          entity.Nickname = request.Nickname;
          entity.TotalCardsFound = request.TotalCardsFound;
          entity.TotalThrophies = request.TotalThrophies;
          entity.TotalWins = request.TotalWins;

          entity.AddDomainEvent (new PlayerUpdatedEvent (entity));
          await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
