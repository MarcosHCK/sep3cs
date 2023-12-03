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
using DataClash.Application.Clans.Commands.CreateClanWithChief;
using DataClash.Application.Clans.Commands.EnterWar;
using DataClash.Application.Clans.Commands.LeaveWar;
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Wars.Commands.CreateWar;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class LeaveWarTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRemoveClan ()
        {
          await RunAsAdministratorAsync ();
          var warId = await SendAsync (new CreateWarCommand { BeginDay = DateTime.Now, Duration = new TimeSpan (1) });

          await RunAsDefaultUserAsync ();
          var clanId = await SendAsync (new CreateClanWithChiefCommand { Description = "Test clan", Name = "Test clan", Region = Region.Anywhere, Type = ClanType.Normal });
          var addCommand = new EnterWarCommand { ClanId = clanId, WarId = warId };
          var removeCommand = new LeaveWarCommand { ClanId = clanId, WarId = warId };

          await SendAsync (addCommand);
          await SendAsync (removeCommand);

          var warClan = await FindAsync<WarClan> (new object[] { clanId, warId });
            warClan.Should ().BeNull ();
        }

      [Test]
      public async Task ShouldRequireClanChiefOrAdministrator ()
        {
          await RunAsAdministratorAsync ();
          var warId = await SendAsync (new CreateWarCommand { BeginDay = DateTime.Now, Duration = new TimeSpan (1) });

          await RunAsDefaultUserAsync ();
          var clanId = await SendAsync (new CreateClanWithChiefCommand { Description = "Test clan", Name = "Test clan", Region = Region.Anywhere, Type = ClanType.Normal });
          var addCommand = new EnterWarCommand { ClanId = clanId, WarId = warId };
          var removeCommand = new LeaveWarCommand { ClanId = clanId, WarId = warId };

          await SendAsync (addCommand);

          await RunAsUserAsync ("other@local", "Otheruser1!", Array.Empty<string> ());
          await FluentActions.Invoking (() => SendAsync (removeCommand)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireMinimunFields ()
        {
          await RunAsDefaultUserAsync ();
          var command = new LeaveWarCommand { };
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ValidationException> ();
        }
    }
}
