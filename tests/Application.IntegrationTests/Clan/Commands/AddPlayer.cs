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
using DataClash.Application.Clans.Commands.CreateClan;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;
using DataClash.Framework.Identity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class AddPlayerTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireAdministrator ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var createClanCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var clanId = await SendAsync (createClanCommand);
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var command = new AddPlayerCommand
            {
              ClanId = clanId,
              PlayerId = playerId,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldAddPlayer ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var createClanCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var clanId = await SendAsync (createClanCommand);
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var command = new AddPlayerCommand
            {
              ClanId = clanId,
              PlayerId = playerId,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          await RunAsAdministratorAsync ();
          await SendAsync (command);

          var item = await FindAsync<PlayerClan> (clanId, playerId);

          item.Should ().NotBeNull ();
          item.Should ().HasProperty ("ClanId");
          item.Should ().HasProperty ("PlayerId");

          item!.ClanId.Should ().Be (command.ClanId);
          item!.PlayerId.Should ().Be (command.PlayerId);
          item!.Role.Should ().Be (command.Role);
        }

      [Test]
      public async Task ShouldNotAddPlayerTwoTimes ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var createClanCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var clanId = await SendAsync (createClanCommand);
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var command = new AddPlayerCommand
            {
              ClanId = clanId,
              PlayerId = playerId,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          await RunAsAdministratorAsync ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<DbUpdateException> ();
        }

      [Test]
      public async Task ShouldNotAddPlayerInTwoClans ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var createClanCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var firstClanId = await SendAsync (createClanCommand);
          var secondClanId = await SendAsync (createClanCommand);
          var user = await FindAsync<ApplicationUser> (userId);
          var playerId = user!.PlayerId!.Value;

          var firstAddPlayerCommand = new AddPlayerCommand
            {
              ClanId = firstClanId,
              PlayerId = playerId,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          var secondAddPlayerCommand = new AddPlayerCommand
            {
              ClanId = secondClanId,
              PlayerId = playerId,
              Role = Domain.Enums.ClanRole.Commoner,
            };

          await RunAsAdministratorAsync ();
          await FluentActions.Invoking (() => SendAsync (firstAddPlayerCommand)).Should ().NotThrowAsync ();
          await FluentActions.Invoking (() => SendAsync (secondAddPlayerCommand)).Should ().ThrowAsync<DbUpdateException> ();
        }
    }
}
