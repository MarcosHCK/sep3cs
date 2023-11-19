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
using DataClash.Framework.Identity;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace DataClash.Framework.Persistence
{
  public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
      private readonly IMediator _mediator;

      public DbSet<Card> Cards => Set<Card> ();
      public DbSet<CardGift> CardGifts => Set<CardGift> ();
      public DbSet<Challenge> Challenges => Set<Challenge> ();
      public DbSet<Clan> Clans => Set<Clan> ();
      public DbSet<MagicCard> MagicCards => Set<MagicCard> ();
      public DbSet<Match> Matches => Set<Match> ();
      public DbSet<Player> Players => Set<Player> ();
      public DbSet<PlayerCard> PlayerCards => Set<PlayerCard> ();
      public DbSet<PlayerChallenge> PlayerChallenges => Set<PlayerChallenge> ();
      public DbSet<PlayerClan> PlayerClans => Set<PlayerClan> ();
      public DbSet<StructCard> StructCards => Set<StructCard> ();
      public DbSet<TroopCard> TroopCards => Set<TroopCard> ();
      public DbSet<War> Wars => Set<War> ();
      public DbSet<WarClan> WarClans => Set<WarClan> ();

      public ApplicationDbContext (
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IMediator mediator)
            : base (options, operationalStoreOptions)
        {
          _mediator = mediator;
        }

      protected override void OnModelCreating (ModelBuilder builder)
        {
          builder.ApplyConfigurationsFromAssembly (Assembly.GetExecutingAssembly ());
          base.OnModelCreating (builder);
        }

      protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
        }

      public override async Task<int> SaveChangesAsync (CancellationToken cancellationToken = default)
        {
          await _mediator.DispatchDomainEvents (this);
          return await base.SaveChangesAsync (cancellationToken);
        }
    }
}
