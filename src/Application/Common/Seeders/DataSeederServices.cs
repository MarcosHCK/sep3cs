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
using System.Text;
using DataClash.Application.Common.Interfaces;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.ValueObjects;

namespace DataClash.Application.Common.Seeders
{
  public class DataSeederProvider : IApplicationDbContextSeederProvider
  {
    private void AddPlayer<T>(IApplicationDbContext context, long? favoriteCardId, long level, string? nickname, long totalCardsFound,
    long totalThrophies, long totalWins, Card? favoriteCard)
    where T : Player, new()
    {
      if (context.Players.All(c => c.Nickname != nickname))
        context.Players.Add(new T
        {
          FavoriteCardId = favoriteCardId,
          Level = level,
          Nickname = nickname,
          TotalCardsFound = totalCardsFound,
          TotalThrophies = totalThrophies,
          TotalWins = totalWins,
          FavoriteCard = favoriteCard
        });
    }

    private void AddChallenge<T>(IApplicationDbContext context, DateTime beginDay, long bounty, long cost, string? description,
    TimeSpan duration, long maxLooses, long minLevel, string? name)
    where T : Challenge, new()
    {
      if (context.Challenges.All(c => c.Name != name))
        context.Challenges.Add(new T
        {
          BeginDay = beginDay,
          Bounty = bounty,
          Cost = cost,
          Description = description,
          Duration = duration,
          MaxLooses = maxLooses,
          MinLevel = minLevel,
          Name = name
        });
    }

    private void AddClan<T>(IApplicationDbContext context, string? description, string? name, Region? region, long totalTrophiesToEnter,
    long totalTrophiesWonOnWar, ClanType type)
    where T : Clan, new()
    {
      if (context.Clans.All(c => c.Name != name))
        context.Clans.Add(new T
        {
         Description = description,
         Name = name ,
         Region = region,  
         TotalTrophiesToEnter = totalTrophiesToEnter,
         TotalTrophiesWonOnWar = totalTrophiesWonOnWar ,
         Type = type
        });
    }

    private void AddWar<T>(IApplicationDbContext context, DateTime beginDay,  TimeSpan duration)
    where T : War, new()
    {
      if (context.Wars.All(c => c.BeginDay != beginDay))
        context.Wars.Add(new T
        {
         BeginDay = beginDay,
         Duration = duration
        });
    }


    public void SeedAsync(IApplicationDbContext context)
    {

      AddClan<Clan>(context,"ganador de guerras", "clan 537", Region.Somewhere, 0, 15, ClanType.Normal);

      DateTime wartime = new DateTime(1,12,7);
      TimeSpan warduration = new TimeSpan(150);
      AddWar<War>(context, wartime , warduration);

    }
  }
}
