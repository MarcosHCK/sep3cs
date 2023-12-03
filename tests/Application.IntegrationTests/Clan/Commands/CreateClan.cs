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
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class CreateClanTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireMinimunFields ()
        {
          await RunAsDefaultUserAsync ();
          var command = new CreateClanCommand ();

          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ValidationException> ();
        }

      [Test]
      public async Task ShouldCreateClan ()
        {
          await RunAsDefaultUserAsync ();
          var command = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Anywhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = Domain.Enums.ClanType.Normal,
            };

          var newId = await SendAsync (command);
          var item = await FindAsync<Clan> (newId);

          item.Should ().NotBeNull ();
          item.Should ().HasProperty ("Description");
          item.Should ().HasProperty ("Name");
          item.Should ().HasProperty ("Region");

          item!.Description.Should ().Be (command.Description);
          item!.Name.Should ().Be (command.Name);
          item!.Region.Should ().Be ((Region) command.Region);
          item!.TotalTrophiesToEnter.Should ().Be (command.TotalTrophiesToEnter);
          item!.TotalTrophiesWonOnWar.Should ().Be (command.TotalTrophiesWonOnWar);
          item!.Type.Should ().Be (command.Type);
        }
    }
}
