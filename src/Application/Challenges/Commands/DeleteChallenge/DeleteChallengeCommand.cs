/* Copyright (c) 2023-2025
 * This file is part of sep3cs.
 *
 * sep3cs is free softChallengese: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free SoftChallengese Foundation, either version 3 of the License, or
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
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Challenges.Commands.DeleteChallenge
{
  [Authorize (Roles = "Administrator")]
  public record DeleteChallengeCommand (long Id) : IRequest;

  public class DeleteChallengeCommandHandler : IRequestHandler<DeleteChallengeCommand>
    {
      private readonly IApplicationDbContext _context;

      public DeleteChallengeCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }

      public async Task Handle (DeleteChallengeCommand request, CancellationToken cancellationToken)
        {
          var entity = await _context.Challenges
            .Where (l => l.Id == request.Id)
            .SingleOrDefaultAsync (cancellationToken)
           ?? throw new NotFoundException (nameof (Challenges), request.Id);

          _context.Challenges.Remove (entity);
          entity.AddDomainEvent (new ChallengeDeletedEvent (entity));

          await _context.SaveChangesAsync (cancellationToken);
        }
    }
}

