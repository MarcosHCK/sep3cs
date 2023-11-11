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

using DataClash.Domain.Common;
using DataClash.Domain.Exceptions;

namespace DataClash.Domain.ValueObjects
{
  public class Region : ValueObject
    {
      public string Code { get; private set; }

      protected static IEnumerable<Region> KnownRegions
        {
          get
          {
            yield return Somewhere;
          }
        }

      protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

      public static Region Somewhere = new ("Somewhere");

      static Region () { }
      private Region () => Code = "und";
      private Region (string code)  => Code = code;

      public static Region From (string code)
        {
          var region = new Region { Code = code };
          if (!KnownRegions.Contains (region))
            throw new UnknownRegionException (code);
        return region;
        }

      public override string ToString ()
        {
            return Code;
        }

      public static explicit operator Region (string code)
        {
            return From (code);
        }

      public static implicit operator string (Region colour)
        {
            return colour.ToString ();
        }
    }
}
