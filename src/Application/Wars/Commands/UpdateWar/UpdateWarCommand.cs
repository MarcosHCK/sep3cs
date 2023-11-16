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
using MediatR;

namespace DataClash.Application.Wars.Commands.UpdateWar
{
  [Authorize (Roles = "Administrator")]
  public record UpdateWarCommand : IRequest
    {
      public long Id { get; init; }
      public DateTime Duration { get; init; }
    }

  public class UpdateWarCommandHandler : IRequestHandler<UpdateWarCommand>
    {
      private readonly IApplicationDbContext _context;

      public UpdateWarCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task Handle (UpdateWarCommand request, CancellationToken cancellationToken)
        {
          var entity = await _context.Wars.FindAsync (new object [] { request.Id }, cancellationToken) ?? throw new NotFoundException (nameof (War), request.Id);
            entity.Duration = request.Duration;
          await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
