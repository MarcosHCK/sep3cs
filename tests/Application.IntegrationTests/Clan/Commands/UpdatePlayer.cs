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
using DataClash.Application.Clans.Commands.UpdatePlayer;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;
using DataClash.Domain.ValueObjects;
using DataClash.Framework.Identity;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Commands
{
  using static Testing;

  public class UpdatePlayerTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldUpdatePlayer ()
        {
          var userId = await RunAsUserAsync ("other@local", "Otheruser1!", Array.Empty<string> ());
          var user = await FindAsync<ApplicationUser> (userId);
            user.Should ().NotBeNull ();
            user.Should ().HasProperty ("PlayerId");
          var playerId = user!.PlayerId!.Value;

          await RunAsDefaultUserAsync ();
          var clanId = await SendAsync (new CreateClanWithChiefCommand { Description = "Test clan", Name = "Test clan", Region = Region.Somewhere, Type = ClanType.Normal });
          var addCommand = new AddPlayerCommand { ClanId = clanId, PlayerId = playerId, Role = ClanRole.Commoner };
          var updateCommand = new UpdatePlayerCommand { ClanId = clanId, PlayerId = playerId, Role = ClanRole.Veteran };

          await SendAsync (addCommand);
          await SendAsync (updateCommand);

          var playerClan = await FindAsync<PlayerClan> (new object[] { clanId, playerId });

          playerClan.Should ().NotBeNull ();
          playerClan!.Role.Should ().Be (updateCommand.Role);
        }

      [Test]
      public async Task ShouldRequireClanChiefOrAdministrator ()
        {
          var userId = await RunAsUserAsync ("other@local", "Otheruser1!", Array.Empty<string> ());
          var user = await FindAsync<ApplicationUser> (userId);
            user.Should ().NotBeNull ();
            user.Should ().HasProperty ("PlayerId");
          var playerId = user!.PlayerId!.Value;

          await RunAsDefaultUserAsync ();
          var clanId = await SendAsync (new CreateClanWithChiefCommand { Description = "Test clan", Name = "Test clan", Region = Region.Somewhere, Type = ClanType.Normal });
          var addCommand = new AddPlayerCommand { ClanId = clanId, PlayerId = playerId, Role = ClanRole.Commoner };
          var updateCommand = new UpdatePlayerCommand { ClanId = clanId, PlayerId = playerId, Role = ClanRole.Veteran };

          await SendAsync (addCommand);
          await RunAsUserAsync ("other2@local", "Otheruser1!", Array.Empty<string> ());
          await FluentActions.Invoking (() => SendAsync (updateCommand)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }
    }
}
