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

namespace DataClash.Application.Statistics.TopPlayersInWars
{
    public class BestPlayers
    {
        public IEnumerable<dynamic> BestPlayer(IApplicationDbContext context)
        {
            var bestPlayers = (from pc in context.PlayerClans
                               join wc in context.WarClans on pc.ClanId equals wc.ClanId
                               join w in context.Wars on wc.WarId equals w.Id
                               join p in context.Players on pc.PlayerId equals p.Id
                               orderby p.TotalThrophies descending
                               select new { Player = p, War = w })
                           .Take(10)
                           .ToList();

            return bestPlayers;
        }

    }

}