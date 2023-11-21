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
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Players.Commands.UpdatePlayer;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Players.Commands
{
  using static Testing;

  public class UpdatePlayerTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldAllowSameUser ()
        {
          var userId = await RunAsDefaultUserAsync ();
          var playerId = await GetPlayerIdForUser (userId);
          var command = new UpdatePlayerCommand
            {
              Id = playerId,
              Nickname = "Testing user",
              Level = 1,
              TotalCardsFound = 0,
              TotalThrophies = 0,
              TotalWins = 0,
            };

          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldAllowOtherUsersFromAdministrator ()
        {
          var userId = await RunAsDefaultUserAsync ();
          var playerId = await GetPlayerIdForUser (userId);
          var command = new UpdatePlayerCommand
            {
              Id = playerId,
              Nickname = "Testing user",
              Level = 1,
              TotalCardsFound = 0,
              TotalThrophies = 0,
              TotalWins = 0,
            };

          await RunAsAdministratorAsync ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldNotAllowOtherUsersFromNonAdministrator ()
        {
          var userId = await RunAsDefaultUserAsync ();
          var playerId = await GetPlayerIdForUser (userId);
          var command = new UpdatePlayerCommand
            {
              Id = playerId,
              Nickname = "Testing user",
              Level = 1,
              TotalCardsFound = 0,
              TotalThrophies = 0,
              TotalWins = 0,
            };

          await RunAsUserAsync ("otheruser@local", "Testing1234!", Array.Empty<string> ());
          await FluentActions.Invoking (() => SendAsync (command)).Should ().NotThrowAsync<ForbiddenAccessException> ();
        }
    }
}
