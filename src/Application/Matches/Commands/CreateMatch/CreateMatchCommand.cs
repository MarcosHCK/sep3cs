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
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.Matches.Commands.CreateMatch
{
    [Authorize (Roles = "Administrator")]
    public record CreateMatchCommand : IRequest<(long, long, DateTime)>
    {
        public long WinnerPlayerId { get; init; }
        public long LooserPlayerId { get; init; }
        public DateTime BeginDate { get; init; }
        public TimeSpan Duration { get; init; }
        //public Player? LooserPlayer { get; init; }
        //public Player? WinnerPlayer { get; init; }

    }
    public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, (long, long, DateTime)>
    {
        private readonly IApplicationDbContext _context;

        public CreateMatchCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Player> GetPlayerById(long PlayerId,  CancellationToken cancellationToken)
        {
            var key = new object [] { PlayerId};
            var entity = await _context.Players.FindAsync (key, cancellationToken) ?? throw new NotFoundException (nameof (Player), key);
            return entity;
        }

        public async Task<(long,long,DateTime)> Handle (CreateMatchCommand request, CancellationToken cancellationToken)
        {
          DateTime ConvertedDate = request.BeginDate.AddHours(-5);
          var entity = new Match
            {
                WinnerPlayerId = request.WinnerPlayerId,
                LooserPlayerId = request.LooserPlayerId,
                BeginDate = ConvertedDate,
                Duration = request.Duration,
                WinnerPlayer = GetPlayerById(request.WinnerPlayerId,cancellationToken).GetAwaiter().GetResult(),
                LooserPlayer = GetPlayerById(request.LooserPlayerId,cancellationToken).GetAwaiter().GetResult()
            };
          
          entity.WinnerPlayer.AddDomainEvent(new MatchCreatedEvent(entity));
          entity.LooserPlayer.AddDomainEvent(new MatchCreatedEvent(entity));
          _context.Matches.Add (entity);

          await _context.SaveChangesAsync (cancellationToken);
          return (entity.WinnerPlayerId,entity.LooserPlayerId,entity.BeginDate);
        }
    }

}