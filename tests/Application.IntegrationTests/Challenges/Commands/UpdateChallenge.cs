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
using DataClash.Application.Challenges.Commands.CreateChallenge;
using DataClash.Application.Challenges.Commands.UpdateChallenge;
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Challenges.Commands
{
  using static Testing;

  public class UpdateChallengeTests : BaseTestFixture
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

          var command = new UpdateChallengeCommand
            {
              Id = itemId,
              BeginDay = DateTime.Now,
              Bounty = 2,
              Cost = 2,
              Description = "Test challenge",
              Duration = new TimeSpan (1),
              MaxLooses = 1,
              MinLevel = 1,
              Name = "Test challenge",
            };

          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireValidChallengeId ()
        {
          await RunAsAdministratorAsync ();
          var command = new UpdateChallengeCommand
            {
              Id = 1,
              BeginDay = DateTime.Now,
              Bounty = 1,
              Cost = 1,
              Description = "Test challenge",
              Duration = new TimeSpan (1),
              MaxLooses = 1,
              MinLevel = 1,
              Name = "Test challenge",
            };

          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<NotFoundException> ();
        }

      [Test]
      public async Task ShouldUpdateChallenge ()
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

          var command = new UpdateChallengeCommand
            {
              Id = itemId,
              BeginDay = DateTime.Now,
              Bounty = 2,
              Cost = 2,
              Description = "Test challenge 2",
              Duration = new TimeSpan (2),
              MaxLooses = 3,
              MinLevel = 3,
              Name = "Test challenge 2",
            };

          await SendAsync (command);
          var item = await FindAsync<Challenge> (itemId);

          item.Should ().HasProperty ("Id");
          item.Should ().HasProperty ("BeginDay");
          item.Should ().HasProperty ("Duration");

          item!.Id.Should ().Be (itemId);
          item!.BeginDay.Should ().Be (command.BeginDay);
          item!.Bounty.Should ().Be (command.Bounty);
          item!.Cost.Should ().Be (command.Cost);
          item!.Description.Should ().Be (command.Description);
          item!.Duration.Should ().Be (command.Duration);
          item!.MaxLooses.Should ().Be (command.MaxLooses);
          item!.MinLevel.Should ().Be (command.MinLevel);
          item!.Name.Should ().Be (command.Name);
        }
    }
}
