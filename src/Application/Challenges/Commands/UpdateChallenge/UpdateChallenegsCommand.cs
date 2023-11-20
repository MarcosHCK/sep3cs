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

namespace DataClash.Application.Challenges.Commands.UpdateChallenges
{
  [Authorize (Roles = "Administrator")]
  public record UpdateChallengesCommand : IRequest
    {
      public long Id { get; init; }
      public DateTime BeginDay { get; init; }
      public long Bounty { get; init; }
      public long Cost { get; init; }
      public string? Description { get; init; } 
      public TimeSpan Duration { get; init; }
      public long MaxLooses { get; init; }
      public long MinLevel { get; init; }
      public string? Name { get; init; }


    }

  public class UpdateChallengesCommandHandler : IRequestHandler<UpdateChallengesCommand>
    {
      private readonly IApplicationDbContext _context;

      public UpdateChallengesCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task Handle (UpdateChallengesCommand request, CancellationToken cancellationToken)
        {
          var entity = await _context.Challenges.FindAsync (new object [] { request.Id }, cancellationToken) ?? throw new NotFoundException (nameof (Challenge), request.Id);
            entity.BeginDay = request.BeginDay;
            entity.Bounty= request.Bounty;
            entity.Cost= request.Cost;
            entity.Description= request.Description;
            entity.Duration= request.Duration;
            entity.MaxLooses= request.MaxLooses;
            entity.MinLevel= request.MinLevel;
            entity.Name= request.Name;


          entity.AddDomainEvent (new ChallengesUpdatedEvent (entity));
          await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
