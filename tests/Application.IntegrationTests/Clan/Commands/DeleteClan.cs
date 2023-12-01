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
using DataClash.Application.Clans.Commands.CreateClan;
using DataClash.Application.Clans.Commands.CreateClanWithChief;
using DataClash.Application.Clans.Commands.DeleteClan;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class DeleteClanTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireAdministratorOrChief ()
        {
          await RunAsDefaultUserAsync ();

          var createCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var command = new DeleteClanCommand (await SendAsync (createCommand));
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireAdministrator ()
        {
          await RunAsDefaultUserAsync ();

          var createCommand = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var command = new DeleteClanCommand (await SendAsync (createCommand));
          await RunAsAdministratorAsync ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireClanChief ()
        {
          await RunAsDefaultUserAsync ();

          var createCommand = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var command = new DeleteClanCommand (await SendAsync (createCommand));
          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldDeleteClan ()
        {
          await RunAsDefaultUserAsync ();

          var createCommand = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var clanId = await SendAsync (createCommand);
          await SendAsync (new DeleteClanCommand (clanId));

          var item = await FindAsync<Clan> (clanId);
          item.Should ().BeNull ();
        }
    }
}
