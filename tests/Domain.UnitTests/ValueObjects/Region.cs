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
using DataClash.Domain.Exceptions;
using DataClash.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Domain.UnitTests.ValueObjects
{
  public class RegionTests
    {
      [Test]
      public void ShouldReturnCorrectRegionCode ()
        {
          var code = Region.Somewhere.Code;
          var region = Region.From (code);

          region.Code.Should ().Be (code);
        }

      [Test]
      public void ToStringReturnsCode ()
        {
          var region = Region.Somewhere;
          region.ToString ().Should ().Be (region.Code);
        }

      [Test]
      public void ShouldPerformImplicitConversionToRegionCodeString ()
      {
        string code = Region.Somewhere;
        code.Should ().Be (Region.Somewhere.Code);
      }

      [Test]
      public void ShouldPerformExplicitConversionGivenKnownRegionCode ()
      {
        var code = Region.Somewhere.Code;
        var region = (Region) code;

        region.Should ().Be (Region.Somewhere);
      }

      [Test]
      public void ShouldThrowUnknownRegionExceptionGivenUnknownRegionCode ()
      {
        FluentActions.Invoking (() => Region.From ("und")).Should ().Throw<UnknownRegionException> ();
      }
    }
}
