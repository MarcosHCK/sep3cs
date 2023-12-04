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
using DataClash.Application.Clans.Queries.GetClanForPlayer;
using DataClash.Domain.Enums;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Clans.Queries.GetClanForCurrentPlayer
{
  using static Testing;

  public class GetClanForCurrentPlayerTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldWorkWithNoPlayer ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var command = new CreateClanCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Anywhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = ClanType.Normal,
            };

          var newId = await SendAsync (command);
          var playerId = await GetPlayerIdForUser (userId);
          var dto = await SendAsync (new GetClanForPlayerQuery (playerId));

          dto.Should ().BeNull ();
        }

      [Test]
      public async Task ShouldWorkWithPlayer ()
        {
          var userId = await RunAsDefaultUserAsync ();

          var command = new CreateClanWithChiefCommand
            {
              Description = "Test clan",
              Name = "Test clan",
              Region = Region.Anywhere,
              TotalTrophiesToEnter = 0,
              TotalTrophiesWonOnWar = 0,
              Type = ClanType.Normal,
            };

          var newId = await SendAsync (command);
          var playerId = await GetPlayerIdForUser (userId);
          var dto = await SendAsync (new GetClanForPlayerQuery (playerId));

          dto.Should ().NotBeNull ();

          dto!.Clan!.Description.Should ().Be (command.Description);
          dto!.Clan!.Id.Should ().Be (newId);
          dto!.Clan!.Name.Should ().Be (command.Name);
          dto!.Clan!.Region.Should ().Be ((Region) command.Region);
          dto!.Clan!.TotalTrophiesToEnter.Should ().Be (command.TotalTrophiesToEnter);
          dto!.Clan!.TotalTrophiesWonOnWar.Should ().Be (command.TotalTrophiesWonOnWar);
          dto!.Clan!.Type.Should ().Be (command.Type);
          dto!.Role.Should ().Be (ClanRole.Chief);
        }
    }
}
