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
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.Matches.Commands.CreateMatch
{
    [Authorize (Roles = "Administrator")]
    public record CreateMatchCommand : IRequest<long>
    {
        public long WinnerPlayerId { get; init; }
        public long LooserPlayerId { get; init; }
        public DateTime BeginDate { get; init; }
        public TimeSpan Duration { get; init; }
        public Player? LooserPlayer { get; init; }
        public Player? WinnerPlayer { get; init; }

    }
    public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand,long>
    {
        private readonly IApplicationDbContext _context;

        public CreateMatchCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> Handle (CreateMatchCommand request, CancellationToken cancellationToken)
        {
          var entity = new Match
            {
                WinnerPlayerId = request.WinnerPlayerId,
                LooserPlayerId = request.LooserPlayerId,
                BeginDate = request.BeginDate,
                Duration = request.Duration,
                WinnerPlayer = request.WinnerPlayer,
                LooserPlayer = request.LooserPlayer
            };

          entity.AddDomainEvent (new MatchCreatedEvent (entity));
          _context.Matches.Add (entity);

          await _context.SaveChangesAsync (cancellationToken);
          return entity.Id;
        }
    }

}