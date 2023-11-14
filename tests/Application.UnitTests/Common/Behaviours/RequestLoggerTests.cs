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
using DataClash.Application.Common.Behaviours;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Wars.Commands.CreateWar;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DataClash.Application.UnitTests.Common.Behaviours
{
  public class RequestLoggerTests
    {
      private Mock<ICurrentUserService> _currentUserService = null!;
      private Mock<IIdentityService> _identityService = null!;
      private Mock<ILogger<CreateWarCommand>> _logger = null!;

        [SetUp]
      public void Setup ()
        {
          _currentUserService = new Mock<ICurrentUserService> ();
          _identityService = new Mock<IIdentityService> ();
          _logger = new Mock<ILogger<CreateWarCommand>> ();
        }

        [Test]
      public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated ()
        {
          _currentUserService.Setup (x => x.UserId).Returns (Guid.NewGuid ().ToString ());

          var requestLogger = new LoggingBehaviour<CreateWarCommand> (_logger.Object, _currentUserService.Object, _identityService.Object);
          await requestLogger.Process (new CreateWarCommand { Duration = DateTime.Now }, new CancellationToken ());
          _identityService.Verify (i => i.GetUserNameAsync (It.IsAny<string> ()), Times.Once);
        }

        [Test]
      public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated ()
        {
          var requestLogger = new LoggingBehaviour<CreateWarCommand> (_logger.Object, _currentUserService.Object, _identityService.Object);
          await requestLogger.Process (new CreateWarCommand { Duration = DateTime.Now }, new CancellationToken ());
          _identityService.Verify (i => i.GetUserNameAsync (It.IsAny<string> ()), Times.Never);
        }
    }
}
