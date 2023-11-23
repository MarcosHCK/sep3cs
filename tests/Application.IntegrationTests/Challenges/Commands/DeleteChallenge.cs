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
using DataClash.Application.Challenges.Commands.CreateChallenge;
using DataClash.Application.Challenges.Commands.DeleteChallenge;
using DataClash.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Challenges.Commands
{
  using static Testing;

  public class DeleteChallengeTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireAdministratorUser ()
        {
          await RunAsAdministratorAsync ();
          var challenge = new CreateChallengeCommand
            {
              BeginDay = DateTime.Now,
              Bounty = 1,
              Cost = 1,
              Description = "Test challenge",
              Duration = new TimeSpan (1),
              MaxLooses = 1,
              MinLevel = 1,
              Name = "Test challenge",
            };

          var itemId = await SendAsync (challenge);
          await RunAsDefaultUserAsync ();
          var command = new DeleteChallengeCommand (itemId);
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireValidChallengeId ()
        {
          var userId = await RunAsAdministratorAsync ();
          var command = new DeleteChallengeCommand (0);
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<NotFoundException> ();
        }

      [Test]
      public async Task ShouldDeleteChallenge ()
        {
          var userId = await RunAsAdministratorAsync ();

          var challenge = new CreateChallengeCommand
            {
              BeginDay = DateTime.Now,
              Bounty = 1,
              Cost = 1,
              Description = "Test challenge",
              Duration = new TimeSpan (1),
              MaxLooses = 1,
              MinLevel = 1,
              Name = "Test challenge",
            };

          var itemId = await SendAsync (challenge);
          await SendAsync (new DeleteChallengeCommand (itemId));
          var item = await FindAsync<Challenge> (itemId);

          item.Should ().BeNull ();
        }
    }
}
