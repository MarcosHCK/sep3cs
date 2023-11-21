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
using DataClash.Application.Wars.Commands.CreateWar;
using DataClash.Application.Wars.Commands.DeleteWar;
using DataClash.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests.Wars.Commands
{
  using static Testing;

  public class DeleteWarTests : BaseTestFixture
    {
      [Test]
      public async Task ShouldRequireAdministratorUser ()
        {
          await RunAsAdministratorAsync ();
          var war = new CreateWarCommand
            {
              BeginDay = DateTime.Now,
              Duration = new TimeSpan (1),
            };

          var itemId = await SendAsync (war);
          await RunAsDefaultUserAsync ();
          var command = new DeleteWarCommand (itemId);
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<ForbiddenAccessException> ();
        }

      [Test]
      public async Task ShouldRequireValidWarId ()
        {
          var userId = await RunAsAdministratorAsync ();
          var command = new DeleteWarCommand (0);
          await FluentActions.Invoking (() => SendAsync (command)).Should ().ThrowAsync<NotFoundException> ();
        }

      [Test]
      public async Task ShouldDeleteWar ()
        {
          var userId = await RunAsAdministratorAsync ();

          var war = new CreateWarCommand
            {
              BeginDay = DateTime.Now,
              Duration = new TimeSpan (1),
            };

          var itemId = await SendAsync (war);
          await SendAsync (new DeleteWarCommand (itemId));
          var item = await FindAsync<War> (itemId);

          item.Should ().BeNull ();
        }
    }
}
