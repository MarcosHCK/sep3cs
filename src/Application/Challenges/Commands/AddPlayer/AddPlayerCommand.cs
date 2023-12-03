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

namespace DataClash.Application.Challenges.Commands.AddPlayer
{
  [Authorize]
  public record AddPlayerCommand : IRequest
    {
      public long ChallengeId { get; init; }
      public long PlayerId { get; init; }
      public long WonThrophies { get; init; }
    }

  public class AddPlayerCommandHandler : IRequestHandler<AddPlayerCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly ICurrentUserService _currentUser;
      private readonly IIdentityService _identityService;

      public AddPlayerCommandHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, ICurrentUserService currentUser, IIdentityService identityService)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _currentUser = currentUser;
          _identityService = identityService;
        }

      public async Task Handle (AddPlayerCommand request, CancellationToken cancellationToken)
        {
          if (_currentPlayer.PlayerId != request.PlayerId
            && await _identityService.IsInRoleAsync (_currentUser.UserId!, Roles.Administrator))
            throw new ForbiddenAccessException ();
          else
            {
              var challenge = await _context.Challenges.FindAsync (new object[] { request.ChallengeId }, cancellationToken)
                              ?? throw new NotFoundException (nameof (Challenge), request.ChallengeId);

              var playerChallenge = new PlayerChallenge
                {
                  ChallengeId = request.ChallengeId,
                  PlayerId = request.PlayerId,
                  WonThrophies = request.WonThrophies,
                };

              challenge.AddDomainEvent (new PlayerAddedEvent<PlayerChallenge> (playerChallenge));

              await _context.PlayerChallenges.AddAsync (playerChallenge, cancellationToken);
              await _context.SaveChangesAsync (cancellationToken);
            }
        }
    }
}
