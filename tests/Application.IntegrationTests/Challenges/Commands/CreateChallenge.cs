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
using DataClash.Application.Common.Exceptions;
using DataClash.Domain.Entities;
using FluentAssertions;
using Namotion.Reflection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Challenges.Commands
{
  using static Testing;

  public class CreateChallengeTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireAdministratorUser ()
        {
          await RunAsDefaultUserAsync ();
          var command = new CreateChallengeCommand ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireMinimumFields ()
        {
          await RunAsAdministratorAsync ();
          var command = new CreateChallengeCommand ();
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ValidationException> ();
        }

      [Test]
      public async Task ShouldCreateChallenge ()
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
          var item = await FindAsync<Challenge> (itemId);

          item.Should ().NotBeNull ();
          item.Should ().HasProperty ("BeginDay");
          item.Should ().HasProperty ("Duration");

          item!.BeginDay.Should ().Be (challenge.BeginDay);
          item!.Bounty.Should ().Be (challenge.Bounty);
          item!.Cost.Should ().Be (challenge.Cost);
          item!.Description.Should ().Be (challenge.Description);
          item!.Duration.Should ().Be (challenge.Duration);
          item!.MaxLooses.Should ().Be (challenge.MaxLooses);
          item!.MinLevel.Should ().Be (challenge.MinLevel);
          item!.Name.Should ().Be (challenge.Name);
        }
    }
}
