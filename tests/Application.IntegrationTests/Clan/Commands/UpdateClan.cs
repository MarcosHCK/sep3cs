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
using DataClash.Application.Clans.Commands.UpdateClan;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class UpdateClanTests : BaseTestFixture
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

          var clanId = await SendAsync (createCommand);

          var command = new UpdateClanCommand
            {
              Id = clanId,

              Description = "Test clan 2",
              Name = "Test clan 2",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 2,
              TotalTrophiesWonOnWar = 2,
              Type = Domain.Enums.ClanType.Normal,
            };

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

          var clanId = await SendAsync (createCommand);

          var command = new UpdateClanCommand
            {
              Id = clanId,

              Description = "Test clan 2",
              Name = "Test clan 2",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 2,
              TotalTrophiesWonOnWar = 2,
              Type = Domain.Enums.ClanType.Normal,
            };

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

          var clanId = await SendAsync (createCommand);

          var command = new UpdateClanCommand
            {
              Id = clanId,

              Description = "Test clan 2",
              Name = "Test clan 2",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 2,
              TotalTrophiesWonOnWar = 2,
              Type = Domain.Enums.ClanType.Normal,
            };

          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldUpdateClan ()
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

          var command = new UpdateClanCommand
            {
              Id = clanId,

              Description = "Test clan 2",
              Name = "Test clan 2",
              Region = Region.Somewhere,
              TotalTrophiesToEnter = 2,
              TotalTrophiesWonOnWar = 2,
              Type = Domain.Enums.ClanType.Normal,
            };

          await SendAsync (command);
          var item = await FindAsync<Clan> (command.Id);

          item.Should ().NotBeNull ();
          item.Should ().HasProperty ("Description");
          item.Should ().HasProperty ("Name");
          item.Should ().HasProperty ("Region");

          item!.Description.Should ().Be (command.Description);
          item!.Name.Should ().Be (command.Name);
          item!.Region.Should ().Be (command.Region);
          item!.TotalTrophiesToEnter.Should ().Be (command.TotalTrophiesToEnter);
          item!.TotalTrophiesWonOnWar.Should ().Be (command.TotalTrophiesWonOnWar);
          item!.Type.Should ().Be (command.Type);
        }
    }
}
