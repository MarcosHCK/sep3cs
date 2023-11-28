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
using DataClash.Application.Clans.Commands.AddPlayer;
using DataClash.Application.Clans.Commands.CreateClanWithChief;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;
using DataClash.Framework.Identity;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class CreateWithChiefTest : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireMinimunFields ()
        {
          await RunAsDefaultUserAsync ();
          var command = new CreateClanWithChiefCommand ();

          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ValidationException> ();
        }

      [Test]
      public async Task ShouldAllowAccessToChief ()
        {
          var user2Id = await RunAsUserAsync ("seconduser@local", "SecondUs3r!", Array.Empty<string> ());
          var user2 = await FindAsync<ApplicationUser> (user2Id);
          var player2Id = user2!.PlayerId!.Value;

          var userId = await RunAsDefaultUserAsync ();
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var createClanWithChiefCommand = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var addPlayerCommand = new AddPlayerCommand
            {
              ClanId = await SendAsync(createClanWithChiefCommand),
              PlayerId = player2Id,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          await FluentActions.Invoking (() => SendAsync (addPlayerCommand)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldNotAllowTwoChiefs ()
        {
          var user2Id = await RunAsUserAsync ("seconduser@local", "SecondUs3r!", Array.Empty<string> ());
          var user2 = await FindAsync<ApplicationUser> (user2Id);
          var player2Id = user2!.PlayerId!.Value;

          var userId = await RunAsDefaultUserAsync ();
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var createClanWithChiefCommand = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var addPlayerCommand = new AddPlayerCommand
            {
              ClanId = await SendAsync(createClanWithChiefCommand),
              PlayerId = player2Id,
              Role = Domain.Enums.ClanRole.Chief,
            };

          await FluentActions.Invoking (() => SendAsync (addPlayerCommand)).Should ().ThrowAsync<ApplicationConstraintException> ();
        }

      [Test]
      public async Task ShouldCreateClanWithChief ()
        {
          var userId = await RunAsDefaultUserAsync ();
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var command = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var clanId = await SendAsync (command);

          var clanItem = await FindAsync<Clan> (clanId);
          var playerClanItem = await FindAsync<PlayerClan> (clanId, playerId);

          clanItem.Should ().NotBeNull ();
          clanItem.Should ().HasProperty ("Description");
          clanItem.Should ().HasProperty ("Name");
          clanItem.Should ().HasProperty ("Region");

          clanItem!.Description.Should ().Be (command.Description);
          clanItem!.Name.Should ().Be (command.Name);
          clanItem!.Region.Should ().Be ((Region) command.Region);
          clanItem!.TotalTrophiesToEnter.Should ().Be (command.TotalTrophiesToEnter);
          clanItem!.TotalTrophiesWonOnWar.Should ().Be (command.TotalTrophiesWonOnWar);
          clanItem!.Type.Should ().Be (command.Type);

          playerClanItem.Should ().NotBeNull ();
          playerClanItem.Should ().HasProperty ("ClanId");
          playerClanItem.Should ().HasProperty ("PlayerId");

          playerClanItem!.ClanId.Should ().Be (clanId);
          playerClanItem!.PlayerId.Should ().Be (playerId);
        }
    }
}
