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
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Statistics.BestPlayer
{
  public record GetBestPlayerQuery(int WarId) : IRequest<List<string[]>>;

  public class GetBestPlayerQueryHandler : IRequestHandler<GetBestPlayerQuery, List<string[]>>
    {
      private readonly IApplicationDbContext _context;

      public GetBestPlayerQueryHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task<List<string[]>> Handle (GetBestPlayerQuery request, CancellationToken cancellationToken)
        {
          var bestPlayers = await (from pc in _context.PlayerClans
                                   join wc in _context.WarClans on pc.ClanId equals wc.ClanId
                                   where wc.WarId == request.WarId
                                   join p in _context.Players on pc.PlayerId equals p.Id
                                   select new { Player = p, Clan = pc.ClanId, Trophies = p.TotalThrophies })
                 .ToListAsync (cancellationToken);
          var groupedPlayers = bestPlayers
              .GroupBy (pc => pc.Clan)
              .Select (g => new { Clan = g.Key, BestPlayer = g.OrderByDescending (x => x.Trophies).First () })
              .ToList ();
          var result = groupedPlayers
              .Select (g => new string[] {
                  g.BestPlayer.Player.Nickname ?? "<no nickname>",
                  _context.Clans.Find (g.Clan)?.Name ?? "<no name>",
                  g.BestPlayer.Trophies.ToString ()
                }).ToList ();
        return result;
        }
    }
}
