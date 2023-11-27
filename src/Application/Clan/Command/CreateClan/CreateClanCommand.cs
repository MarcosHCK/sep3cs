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
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.Events;
using DataClash.Domain.ValueObjects;
using MediatR;

namespace DataClash.Application.Clans.Commands.CreateClan
{
  [Authorize]
  public record CreateClanCommand : IRequest<long>
    {
      public string? Description { get; init; }
      public string? Name { get; init; }
      public string? Region { get; init; }
      public long TotalTrophiesToEnter { get; init; }
      public long TotalTrophiesWonOnWar { get; init; }
      public ClanType Type { get; init; }
    }

  public class CreateClanCommandHandler : IRequestHandler<CreateClanCommand, long>
    {
      private readonly IApplicationDbContext _context;

      public CreateClanCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task<long> Handle (CreateClanCommand request, CancellationToken cancellationToken)
        {
          var entity = new Clan
            {
              Description = request.Description,
              Name = request.Name,
              Region = Region.From (request.Region!),
              TotalTrophiesToEnter = request.TotalTrophiesToEnter,
              TotalTrophiesWonOnWar = request.TotalTrophiesWonOnWar,
              Type = request.Type,
            };

          entity.AddDomainEvent (new ClanCreatedEvent (entity));
          _context.Clans.Add (entity);

          await _context.SaveChangesAsync (cancellationToken);
        return entity.Id;
        }
    }
}
