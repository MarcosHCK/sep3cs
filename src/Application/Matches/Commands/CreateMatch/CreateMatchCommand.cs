/* sep3cs is free software: you can redistribute it and/or modify
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

namespace DataClash.Application.Matches.Commands.CreateMatch
{
  [Authorize (Roles = Roles.Administrator)]
  public record CreateMatchCommand : IRequest
    {
      public long WinnerPlayerId { get; init; }
      public long LooserPlayerId { get; init; }
      public DateTime BeginDate { get; init; }
      public TimeSpan Duration { get; init; }
    }

  public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand>
    {
      private readonly IApplicationDbContext _context;

      public CreateMatchCommandHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task Handle (CreateMatchCommand request, CancellationToken cancellationToken)
        {
          var match = new Match
            {
              WinnerPlayerId = request.WinnerPlayerId,
              LooserPlayerId = request.LooserPlayerId,
              BeginDate = request.BeginDate,
              Duration = request.Duration,
            };

          var winnerPlayer = await _context.Players.FindAsync (new object[] { request.WinnerPlayerId }, cancellationToken)
                              ?? throw new NotFoundException (nameof (Player), new object[] { request.WinnerPlayerId });
          var looserPlayer = await _context.Players.FindAsync (new object[] { request.LooserPlayerId }, cancellationToken)
                              ?? throw new NotFoundException (nameof (Player), new object[] { request.LooserPlayerId });

          winnerPlayer.AddDomainEvent (new MatchCreatedEvent (match));
          looserPlayer.AddDomainEvent (new MatchCreatedEvent (match));
          _context.Matches.Add (match);

          await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
