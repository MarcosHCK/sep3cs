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
using DataClash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Common.Interfaces
{
  public interface IApplicationDbContext
    {
      public DbSet<Card> Cards { get; }
      public DbSet<CardGift> CardGifts { get; }
      public DbSet<Challenge> Challenges { get; }
      public DbSet<Clan> Clans { get; }
      public DbSet<MagicCard> MagicCards { get; }
      public DbSet<Match> Matches { get; }
      public DbSet<Player> Players { get; }
      public DbSet<PlayerCard> PlayerCards { get; }
      public DbSet<PlayerChallenge> PlayerChallenges { get; }
      public DbSet<PlayerClan> PlayerClans { get; }
      public DbSet<StructCard> StructCards { get; }
      public DbSet<TroopCard> TroopCards { get; }
      public DbSet<War> Wars { get; }
      public DbSet<WarClan> WarClans { get; }

      Task<int> SaveChangesAsync (CancellationToken cancellationToken);
    }
}
