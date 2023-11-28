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

namespace DataClash.Application.Statistics.CompletedChallenges
{
    public class CompletedChallenges
    {
        public IEnumerable<Tuple<Player?, Challenge?>> GetCompletedChallenge(IApplicationDbContext context)
        {
            var completedChallenges = context.PlayerChallenges
                .GroupBy(pc => pc.ChallengeId)
                .Where(g => g.Sum(pc => pc.WonThrophies) == g.First().Challenge.Bounty)
                .SelectMany(g => g.Select(pc => new { Player = pc.Player, Challenge = pc.Challenge }))
                .ToList();

            return completedChallenges.Select(x => Tuple.Create(x.Player, x.Challenge));
        }
    }
}