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
using DataClash.Domain.Entities;

namespace DataClash.Application.Statistics.TopPlayersInWars
{
    public class BestPlayers
    {
        public IEnumerable<Tuple<Player, Clan, long>> BestPlayer(IApplicationDbContext context, int warId)
        {
            var bestPlayers = (from pc in context.PlayerClans
                               join wc in context.WarClans on pc.ClanId equals wc.ClanId
                               join w in context.Wars on wc.WarId equals w.Id
                               join p in context.Players on pc.PlayerId equals p.Id
                               where w.Id == warId
                               let clan = context.Clans.FirstOrDefault(c => c.Id == pc.ClanId)
                               group new { Player = p, Clan = clan, Trophies = p.TotalThrophies } by clan into g
                               select new Tuple<Player, Clan, long>(g.OrderByDescending(x => x.Trophies).First().Player, g.Key, g.OrderByDescending(x => x.Trophies).First().Trophies))
                         .ToList();

            return bestPlayers;
        }

        public IEnumerable<long> GetAllWarIds(IApplicationDbContext context)
        {
            var allWarIds = context.Wars.Select(w => w.Id).ToList();
            return allWarIds;
        }


    }
}