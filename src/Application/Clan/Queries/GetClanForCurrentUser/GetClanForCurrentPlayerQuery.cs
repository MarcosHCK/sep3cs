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
using DataClash.Application.Clans.Queries.GetClansWithPagination;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using MediatR;
using AutoMapper;
using FluentValidation;
using System.Data;
using DataClash.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Clans.Queries.GetClanForCurrentPlayer
{
  [Authorize]
  public record GetClanForCurrentPlayerQuery () : IRequest<ClanBriefDto?>;

  public class GetClanForCurrentPlayerQueryHandler : IRequestHandler<GetClanForCurrentPlayerQuery, ClanBriefDto?>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentPlayerService _currentPlayer;
      private readonly IMapper _mapper;

      public GetClanForCurrentPlayerQueryHandler (IApplicationDbContext context, ICurrentPlayerService currentPlayer, IMapper mapper)
        {
          _context = context;
          _currentPlayer = currentPlayer;
          _mapper = mapper;
        }

      public async Task<ClanBriefDto?> Handle (GetClanForCurrentPlayerQuery query, CancellationToken cancellationToken)
        {
          var playerIdProxy = _currentPlayer.PlayerId;
          long playerId;

          if (!playerIdProxy.HasValue)
            throw new ApplicationConstraintException ("User is not a player");
          else
            playerId = playerIdProxy.Value;

          var playerClan = await _context.PlayerClans.Where (e => e.PlayerId == playerId).FirstOrDefaultAsync (cancellationToken);
          var clan = playerClan == null ? null : await _context.Clans.FindAsync (new object[] { playerClan.ClanId }, cancellationToken);
        return clan == null ? null : _mapper.Map<ClanBriefDto> (clan);
        }
    }
}
