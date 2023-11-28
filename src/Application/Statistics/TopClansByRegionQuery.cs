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
using DataClash.Domain.ValueObjects;

namespace DataClash.Application.Statistics.TopClansByRegion
{
    public class BestClans
    {
        public async Task<IEnumerable<Tuple<Clan, Region, long>>> GetTopClansByRegion(IApplicationDbContext context)
        {
            var topClans = context.Clans
                .GroupBy(c => c.Region.Code)
                .Select(g => g.OrderByDescending(c => c.TotalTrophiesWonOnWar).First())
                .Select(c => Tuple.Create(c, c.Region, c.TotalTrophiesWonOnWar))
                .ToList();

            return topClans;
        }


    }
}