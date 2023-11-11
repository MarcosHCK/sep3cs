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

namespace DataClash.Domain.Common
{
  public abstract class ValueObject
    {
      protected abstract IEnumerable<object> GetEqualityComponents();

      protected static bool EqualOperator (ValueObject left, ValueObject right)
        {
          if (left is null ^ right is null)
            return false;
        return left?.Equals (right!) != false;
        }

      public override bool Equals (object? obj)
        {
          if (obj == null || obj.GetType () != GetType ())
            return false;
        return GetEqualityComponents ().SequenceEqual (((ValueObject) obj).GetEqualityComponents ());
        }

      public override int GetHashCode ()
        {
          return GetEqualityComponents ().Select (x => x != null ? x.GetHashCode () : 0).Aggregate ((x, y) => x ^ y);
        }

      protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
          return !(EqualOperator (left, right));
        }
    }
}
