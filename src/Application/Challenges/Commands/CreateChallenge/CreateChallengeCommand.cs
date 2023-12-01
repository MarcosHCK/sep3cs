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
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.Challenges.Commands.CreateChallenge
{
  [Authorize (Roles = "Administrator")]
  public record CreateChallengeCommand : IRequest<long>
    {
      public DateTime BeginDay { get; init; }
      public long Bounty { get; init; }
      public long Cost { get; init; }
      public string? Description { get; init; }
      public TimeSpan Duration { get; init; }
      public long MaxLooses { get; init; }
      public long MinLevel { get; init; }
      public string Name { get; init; } = null!;

    }

  public class CreateChallengeCommandHandler : IRequestHandler<CreateChallengeCommand, long>
    {
      private readonly IApplicationDbContext _context;

      public CreateChallengeCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task<long> Handle (CreateChallengeCommand request, CancellationToken cancellationToken)
        {
          var entity = new Challenge
            {
              BeginDay = request.BeginDay,
              Bounty = request.Bounty,
              Cost = request.Cost,
              Description = request.Description,
              Duration = request.Duration,
              MaxLooses = request.MaxLooses,
              MinLevel = request.MinLevel,
              Name = request.Name
            };

          entity.AddDomainEvent (new ChallengeCreatedEvent (entity));
          _context.Challenges.Add (entity);

          await _context.SaveChangesAsync (cancellationToken);
        return entity.Id;
        }
    }
}
